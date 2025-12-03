using Newtonsoft.Json;

namespace CryptoJackpotService.Worker.Infrastructure;

/// <summary>
/// Configuración compartida para serialización JSON en consumers de Kafka
/// </summary>
internal static class KafkaJsonSettings
{
    /// <summary>
    /// Configuración de serialización JSON robusta compartida entre todos los consumers
    /// </summary>
    public static readonly JsonSerializerSettings Settings = new()
    {
        MissingMemberHandling = MissingMemberHandling.Ignore,
        NullValueHandling = NullValueHandling.Ignore
    };
}

