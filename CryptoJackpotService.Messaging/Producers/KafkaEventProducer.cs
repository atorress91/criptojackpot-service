using Confluent.Kafka;
using CryptoJackpotService.Messaging.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace CryptoJackpotService.Messaging.Producers;

public class KafkaEventProducer : IEventProducer, IDisposable
{
    private readonly IProducer<string, string> _producer;
    private readonly ILogger<KafkaEventProducer> _logger;

    public KafkaEventProducer(IOptions<KafkaSettings> kafkaSettings, ILogger<KafkaEventProducer> logger)
    {
        _logger = logger;
        
        var config = new ProducerConfig
        {
            BootstrapServers = kafkaSettings.Value.BootstrapServers,
            Acks = Acks.All, // Requerido cuando EnableIdempotence = true
            EnableIdempotence = true,
            MaxInFlight = 5,
            MessageSendMaxRetries = 10,
            RetryBackoffMs = 1000
        };

        _producer = new ProducerBuilder<string, string>(config).Build();
    }

    public async Task PublishAsync<TEvent>(TEvent @event, string topic, CancellationToken cancellationToken = default) 
        where TEvent : class
    {
        try
        {
            var eventJson = JsonConvert.SerializeObject(@event, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                NullValueHandling = NullValueHandling.Ignore
            });

            var message = new Message<string, string>
            {
                Key = Guid.NewGuid().ToString(),
                Value = eventJson,
                Timestamp = Timestamp.Default
            };

            var deliveryResult = await _producer.ProduceAsync(topic, message, cancellationToken);

            _logger.LogInformation(
                "Event {EventType} published to topic {Topic} at partition {Partition} with offset {Offset}",
                typeof(TEvent).Name,
                topic,
                deliveryResult.Partition.Value,
                deliveryResult.Offset.Value);
        }
        catch (ProduceException<string, string> ex)
        {
            _logger.LogError(ex, 
                "Failed to publish event {EventType} to topic {Topic}: {Error}", 
                typeof(TEvent).Name, 
                topic, 
                ex.Error.Reason);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, 
                "Unexpected error publishing event {EventType} to topic {Topic}", 
                typeof(TEvent).Name, 
                topic);
            throw;
        }
    }

    public void Dispose()
    {
        _producer.Flush(TimeSpan.FromSeconds(10));
        _producer.Dispose();
    }
}

