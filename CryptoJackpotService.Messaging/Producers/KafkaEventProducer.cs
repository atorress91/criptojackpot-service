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
    private bool _disposed;
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

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_disposed)
            return;

        if (disposing)
        {
            _producer.Flush(TimeSpan.FromSeconds(10));
            _producer.Dispose();
        }

        _disposed = true;
    }
}