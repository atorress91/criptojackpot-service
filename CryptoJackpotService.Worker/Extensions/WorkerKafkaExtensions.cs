using CryptoJackpotService.Messaging.Configuration;
using CryptoJackpotService.Messaging.Consumers;
using CryptoJackpotService.Worker.Infrastructure;
using Microsoft.Extensions.Options;

namespace CryptoJackpotService.Worker.Extensions;

/// <summary>
/// Extensión para registrar automáticamente los consumidores de Kafka en el Worker
/// </summary>
public static class WorkerKafkaExtensions
{
    /// <summary>
    /// Registra un consumidor de Kafka completo (handler + worker) para un tipo de evento específico
    /// </summary>
    public static IServiceCollection AddKafkaConsumer<TEvent, TEventHandler>(
        this IServiceCollection services)
        where TEvent : class
        where TEventHandler : class, IEventHandler<TEvent>
    {
        // Registrar el handler
        services.AddScoped<IEventHandler<TEvent>, TEventHandler>();

        // Registrar el worker como HostedService
        services.AddHostedService(serviceProvider =>
        {
            var logger = serviceProvider.GetRequiredService<ILogger<GenericKafkaConsumerWorker<TEvent>>>();
            var kafkaSettings = serviceProvider.GetRequiredService<IOptions<KafkaSettings>>();
            var scopeFactory = serviceProvider.GetRequiredService<IServiceScopeFactory>();
            var topicMapper = serviceProvider.GetRequiredService<IEventTopicMapper>();

            var topic = topicMapper.GetTopicForEvent<TEvent>();
            var consumerGroupSuffix = topicMapper.GetConsumerGroupSuffix<TEvent>();

            return new GenericKafkaConsumerWorker<TEvent>(
                logger,
                kafkaSettings,
                scopeFactory,
                topic,
                consumerGroupSuffix);
        });

        return services;
    }
}

