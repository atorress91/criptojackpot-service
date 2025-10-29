using CryptoJackpotService.Messaging.Events;
using Microsoft.Extensions.Options;

namespace CryptoJackpotService.Messaging.Configuration;

/// <summary>
/// Implementación del mapper de eventos a topics
/// Centraliza el mapeo de eventos a topics de Kafka
/// </summary>
public class EventTopicMapper : IEventTopicMapper
{
    private readonly Dictionary<Type, string> _eventTopicMap = new();
    private readonly Dictionary<Type, string> _eventConsumerGroupMap = new();
    private readonly KafkaSettings _kafkaSettings;

    public EventTopicMapper(IOptions<KafkaSettings> kafkaSettings)
    {
        _kafkaSettings = kafkaSettings.Value;
        InitializeEventMappings();
    }

    private void InitializeEventMappings()
    {
        // Mapeo de eventos a topics
        RegisterEvent<UserCreatedEvent>(
            _kafkaSettings.UserCreatedTopic,
            "user-created");

        RegisterEvent<PasswordResetRequestedEvent>(
            _kafkaSettings.PasswordResetTopic,
            "password-reset");

        // Agregar más eventos aquí según sea necesario
        // RegisterEvent<NuevoEvento>(_kafkaSettings.NuevoTopic, "nuevo-sufijo");
    }

    private void RegisterEvent<TEvent>(string topic, string consumerGroupSuffix) where TEvent : class
    {
        var eventType = typeof(TEvent);
        _eventTopicMap[eventType] = topic;
        _eventConsumerGroupMap[eventType] = consumerGroupSuffix;
    }

    public string GetTopicForEvent<TEvent>() where TEvent : class
    {
        var eventType = typeof(TEvent);
        if (_eventTopicMap.TryGetValue(eventType, out var topic))
        {
            return topic;
        }

        throw new InvalidOperationException(
            $"No topic mapping found for event type {eventType.Name}. " +
            "Please register the event in EventTopicMapper.InitializeEventMappings()");
    }

    public string GetConsumerGroupSuffix<TEvent>() where TEvent : class
    {
        var eventType = typeof(TEvent);
        if (_eventConsumerGroupMap.TryGetValue(eventType, out var suffix))
        {
            return suffix;
        }

        // Default: usar el nombre del evento en minúsculas
        return eventType.Name.ToLower();
    }
}

