using Confluent.Kafka;
using CryptoJackpotService.Messaging.Configuration;
using CryptoJackpotService.Messaging.Consumers;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Polly;
using Polly.Retry;

namespace CryptoJackpotService.Worker.Infrastructure;


/// <summary>
/// Worker genérico que consume eventos de Kafka y los delega a un handler específico
/// </summary>
/// <typeparam name="TEvent">Tipo del evento a consumir</typeparam>
public class GenericKafkaConsumerWorker<TEvent>(
    ILogger<GenericKafkaConsumerWorker<TEvent>> logger,
    IOptions<KafkaSettings> kafkaSettings,
    IServiceScopeFactory serviceScopeFactory,
    string topic,
    string? consumerGroupSuffix = null,
    int maxRetryAttempts = 3)
    : BackgroundService
    where TEvent : class
{
    private readonly KafkaSettings _kafkaSettings = kafkaSettings.Value;
    private readonly string _consumerGroupSuffix = consumerGroupSuffix ?? typeof(TEvent).Name.ToLower();
    private readonly int _maxRetryAttempts = maxRetryAttempts;
    private IConsumer<string, string>? _consumer;
    
    /// <summary>
    /// Indica si el consumer está funcionando correctamente (para health checks)
    /// </summary>
    public bool IsHealthy { get; private set; } = true;
    
    /// <summary>
    /// Última vez que se procesó un mensaje exitosamente
    /// </summary>
    public DateTime? LastSuccessfulProcessing { get; private set; }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation(
            "{ConsumerName} starting for topic {Topic}...",
            typeof(TEvent).Name,
            topic);

        var config = new ConsumerConfig
        {
            BootstrapServers = _kafkaSettings.BootstrapServers,
            GroupId = $"{_kafkaSettings.GroupId}-{_consumerGroupSuffix}",
            EnableAutoCommit = _kafkaSettings.EnableAutoCommit,
            AutoOffsetReset = AutoOffsetReset.Latest,
            SessionTimeoutMs = _kafkaSettings.SessionTimeoutMs,
            EnableAutoOffsetStore = false
        };

        _consumer = new ConsumerBuilder<string, string>(config).Build();
        _consumer.Subscribe(topic);

        logger.LogInformation(
            "Subscribed to topic: {Topic} with consumer group: {GroupId}",
            topic,
            config.GroupId);

        // Usar Task.Run para no bloquear el thread de inicio
        await Task.Run(() => RunConsumerLoopAsync(stoppingToken), stoppingToken);
    }

    private async Task ProcessEventAsync(TEvent @event, CancellationToken cancellationToken)
    {
        using var scope = serviceScopeFactory.CreateScope();
        var handler = scope.ServiceProvider.GetService<IEventHandler<TEvent>>();

        if (handler == null)
        {
            logger.LogError(
                "No handler registered for event type {EventType}",
                typeof(TEvent).Name);
            throw new InvalidOperationException(
                $"No handler registered for event type {typeof(TEvent).Name}");
        }

        await handler.HandleAsync(@event, cancellationToken);
    }

    /// <summary>
    /// Procesa el evento con retry policy usando Polly
    /// </summary>
    private async Task<bool> ProcessEventWithRetryAsync(TEvent @event, CancellationToken cancellationToken)
    {
        var retryPipeline = new ResiliencePipelineBuilder()
            .AddRetry(new RetryStrategyOptions
            {
                MaxRetryAttempts = _maxRetryAttempts,
                Delay = TimeSpan.FromSeconds(2),
                BackoffType = DelayBackoffType.Exponential,
                UseJitter = true,
                OnRetry = args =>
                {
                    logger.LogWarning(
                        args.Outcome.Exception,
                        "Retry {RetryCount}/{MaxRetries} for {EventType} after {Delay}s. Error: {Error}",
                        args.AttemptNumber,
                        _maxRetryAttempts,
                        typeof(TEvent).Name,
                        args.RetryDelay.TotalSeconds,
                        args.Outcome.Exception?.Message ?? "Unknown error");
                    return ValueTask.CompletedTask;
                }
            })
            .Build();

        try
        {
            await retryPipeline.ExecuteAsync(
                async token => await ProcessEventAsync(@event, token),
                cancellationToken);
            return true;
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex,
                "All {MaxRetries} retry attempts failed for {EventType}. Message will be skipped.",
                _maxRetryAttempts,
                typeof(TEvent).Name);
            return false;
        }
    }

    private async Task RunConsumerLoopAsync(CancellationToken stoppingToken)
    {
        if (_consumer == null)
        {
            logger.LogError("Consumer is not initialized");
            IsHealthy = false;
            return;
        }

        try
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await ConsumeAndProcessMessageAsync(stoppingToken);
            }
        }
        catch (OperationCanceledException ex)
        {
            logger.LogInformation(
                ex,
                "{ConsumerName} is shutting down gracefully",
                typeof(TEvent).Name);
        }
        finally
        {
            _consumer?.Close();
        }
    }

    private async Task ConsumeAndProcessMessageAsync(CancellationToken stoppingToken)
    {
        try
        {
            var consumeResult = _consumer!.Consume(stoppingToken);

            if (consumeResult?.Message?.Value == null)
                return;

            logger.LogInformation(
                "Received message from topic {Topic}, partition {Partition}, offset {Offset}",
                consumeResult.Topic,
                consumeResult.Partition.Value,
                consumeResult.Offset.Value);

            await ProcessConsumedMessageAsync(consumeResult, stoppingToken);
        }
        catch (ConsumeException ex)
        {
            await HandleConsumeExceptionAsync(ex, stoppingToken);
        }
        catch (Exception ex) when (ex is not OperationCanceledException)
        {
            logger.LogError(
                ex,
                "Unexpected error processing message from topic {Topic}",
                topic);
        }
    }

    private async Task ProcessConsumedMessageAsync(ConsumeResult<string, string> consumeResult, CancellationToken stoppingToken)
    {
        var @event = JsonConvert.DeserializeObject<TEvent>(consumeResult.Message.Value, KafkaJsonSettings.Settings);

        if (@event == null)
        {
            logger.LogWarning(
                "Failed to deserialize message from offset {Offset}, skipping...",
                consumeResult.Offset.Value);
            _consumer!.StoreOffset(consumeResult);
            return;
        }

        var success = await ProcessEventWithRetryAsync(@event, stoppingToken);
        _consumer!.StoreOffset(consumeResult);

        LogProcessingResult(success, consumeResult.Offset.Value);
    }

    private void LogProcessingResult(bool success, long offset)
    {
        if (success)
        {
            LastSuccessfulProcessing = DateTime.UtcNow;
            IsHealthy = true;
            
            logger.LogInformation(
                "Successfully processed {EventType} from offset {Offset}",
                typeof(TEvent).Name,
                offset);
        }
        else
        {
            logger.LogWarning(
                "Message from offset {Offset} was skipped after all retries failed",
                offset);
        }
    }

    private async Task HandleConsumeExceptionAsync(ConsumeException ex, CancellationToken stoppingToken)
    {
        if (ex.Error.Code == ErrorCode.UnknownTopicOrPart)
        {
            logger.LogWarning(
                "Topic {Topic} does not exist yet. Waiting 5 seconds before retry... (Topic will be auto-created when first message is produced)",
                topic);
            await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
            return;
        }
        
        IsHealthy = false;
        logger.LogError(
            ex,
            "Error consuming message from topic {Topic}: {Error}",
            topic,
            ex.Error.Reason);
    }

    /// <summary>
    /// Graceful shutdown mejorado
    /// </summary>
    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "{ConsumerName} stopping gracefully...",
            typeof(TEvent).Name);

        // Dar tiempo para procesar mensajes en vuelo
        try
        {
            await Task.Delay(TimeSpan.FromSeconds(2), cancellationToken);
        }
        catch (OperationCanceledException)
        {
            // Ignorar si el token fue cancelado durante el delay
        }

        _consumer?.Close();
        await base.StopAsync(cancellationToken);
        
        logger.LogInformation(
            "{ConsumerName} stopped successfully",
            typeof(TEvent).Name);
    }

    public override void Dispose()
    {
        _consumer?.Dispose();
        base.Dispose();
        GC.SuppressFinalize(this);
    }
}