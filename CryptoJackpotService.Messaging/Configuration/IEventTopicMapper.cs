namespace CryptoJackpotService.Messaging.Configuration;

/// <summary>
/// Define el mapeo entre tipos de eventos y sus topics de Kafka
/// </summary>
public interface IEventTopicMapper
{
    /// <summary>
    /// Obtiene el nombre del topic para un tipo de evento específico
    /// </summary>
    string GetTopicForEvent<TEvent>() where TEvent : class;
    
    /// <summary>
    /// Obtiene el sufijo del consumer group para un tipo de evento específico
    /// </summary>
    string GetConsumerGroupSuffix<TEvent>() where TEvent : class;
}

