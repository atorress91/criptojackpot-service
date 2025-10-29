namespace CryptoJackpotService.Messaging.Producers;

public interface IEventProducer
{
    Task PublishAsync<TEvent>(TEvent @event, string topic, CancellationToken cancellationToken = default) 
        where TEvent : class;
}

