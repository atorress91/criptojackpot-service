using CryptoJackpotService.Messaging.Consumers;
using Microsoft.Extensions.DependencyInjection;

namespace CryptoJackpotService.Messaging.Configuration;

/// <summary>
/// Extensión para registrar automáticamente los consumidores de Kafka
/// El worker genérico debe estar en el proyecto Worker
/// </summary>
public static class KafkaConsumerExtensions
{
    /// <summary>
    /// Registra un handler de evento para Kafka
    /// Nota: El worker genérico se debe registrar en el proyecto Worker debido a dependencias de BackgroundService
    /// </summary>
    public static IServiceCollection AddKafkaEventHandler<TEvent, TEventHandler>(
        this IServiceCollection services)
        where TEvent : class
        where TEventHandler : class, IEventHandler<TEvent>
    {
        services.AddScoped<IEventHandler<TEvent>, TEventHandler>();
        return services;
    }
}

