using Confluent.Kafka;
using CryptoJackpotService.Messaging.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace CryptoJackpotService.Messaging.Consumers;

public class KafkaEventConsumer<TEvent> : IDisposable where TEvent : class
{
    private readonly IConsumer<string, string> _consumer;
    private readonly IEventHandler<TEvent> _eventHandler;
    private readonly ILogger<KafkaEventConsumer<TEvent>> _logger;
    private readonly string _topic;

    public KafkaEventConsumer(
        KafkaSettings kafkaSettings,
        IEventHandler<TEvent> eventHandler,
        ILogger<KafkaEventConsumer<TEvent>> logger,
        string topic)
    {
        _eventHandler = eventHandler;
        _logger = logger;
        _topic = topic;

        var config = new ConsumerConfig
        {
            BootstrapServers = kafkaSettings.BootstrapServers,
            GroupId = kafkaSettings.GroupId,
            EnableAutoCommit = kafkaSettings.EnableAutoCommit,
            AutoOffsetReset = AutoOffsetReset.Earliest,
            SessionTimeoutMs = kafkaSettings.SessionTimeoutMs,
            EnableAutoOffsetStore = false
        };

        _consumer = new ConsumerBuilder<string, string>(config).Build();
        _consumer.Subscribe(_topic);
    }

    public async Task StartConsumingAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting Kafka consumer for topic {Topic}", _topic);

        try
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    var consumeResult = _consumer.Consume(cancellationToken);

                    if (consumeResult?.Message?.Value == null)
                        continue;

                    _logger.LogInformation(
                        "Received message from topic {Topic}, partition {Partition}, offset {Offset}",
                        consumeResult.Topic,
                        consumeResult.Partition.Value,
                        consumeResult.Offset.Value);

                    var eventData = JsonConvert.DeserializeObject<TEvent>(consumeResult.Message.Value);

                    if (eventData != null)
                    {
                        await _eventHandler.HandleAsync(eventData, cancellationToken);
                        _consumer.StoreOffset(consumeResult);
                        
                        _logger.LogInformation(
                            "Successfully processed message from topic {Topic}, offset {Offset}",
                            consumeResult.Topic,
                            consumeResult.Offset.Value);
                    }
                }
                catch (ConsumeException ex)
                {
                    _logger.LogError(ex, "Error consuming message from topic {Topic}: {Error}", _topic, ex.Error.Reason);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error processing message from topic {Topic}", _topic);
                }
            }
        }
        catch (OperationCanceledException)
        {
            _logger.LogInformation("Kafka consumer for topic {Topic} is shutting down", _topic);
        }
        finally
        {
            _consumer.Close();
        }
    }

    public void Dispose()
    {
        _consumer?.Dispose();
    }
}

