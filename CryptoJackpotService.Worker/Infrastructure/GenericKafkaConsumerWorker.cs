using Confluent.Kafka;
using CryptoJackpotService.Messaging.Configuration;
using CryptoJackpotService.Messaging.Consumers;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

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
    string? consumerGroupSuffix = null)
    : BackgroundService
    where TEvent : class
{
    private readonly KafkaSettings _kafkaSettings = kafkaSettings.Value;
    private readonly string _consumerGroupSuffix = consumerGroupSuffix ?? typeof(TEvent).Name.ToLower();
    private IConsumer<string, string>? _consumer;

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
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

        return RunConsumerLoopAsync(stoppingToken);
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

    private async Task RunConsumerLoopAsync(CancellationToken stoppingToken)
    {
        if (_consumer == null)
        {
            logger.LogError("Consumer is not initialized");
            return;
        }

        try
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var consumeResult = _consumer.Consume(stoppingToken);

                    if (consumeResult?.Message?.Value == null)
                        continue;

                    logger.LogInformation(
                        "Received message from topic {Topic}, partition {Partition}, offset {Offset}",
                        consumeResult.Topic,
                        consumeResult.Partition.Value,
                        consumeResult.Offset.Value);

                    var @event = JsonConvert.DeserializeObject<TEvent>(consumeResult.Message.Value);

                    if (@event != null)
                    {
                        await ProcessEventAsync(@event, stoppingToken);
                        _consumer.StoreOffset(consumeResult);

                        logger.LogInformation(
                            "Successfully processed {EventType} from offset {Offset}",
                            typeof(TEvent).Name,
                            consumeResult.Offset.Value);
                    }
                }
                catch (ConsumeException ex)
                {
                    logger.LogError(
                        ex,
                        "Error consuming message from topic {Topic}: {Error}",
                        topic,
                        ex.Error.Reason);
                }
                catch (Exception ex)
                {
                    logger.LogError(
                        ex,
                        "Error processing message from topic {Topic}",
                        topic);
                }
            }
        }
        catch (OperationCanceledException ex)
        {
            logger.LogInformation(
                ex,
                "{ConsumerName} is shutting down",
                typeof(TEvent).Name);
        }
        finally
        {
            _consumer?.Close();
        }
    }

    public override void Dispose()
    {
        _consumer?.Dispose();
        base.Dispose();
        GC.SuppressFinalize(this);
    }
}