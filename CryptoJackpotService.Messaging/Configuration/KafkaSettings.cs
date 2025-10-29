namespace CryptoJackpotService.Messaging.Configuration;

public class KafkaSettings
{
    public string BootstrapServers { get; set; } = "localhost:9092";
    public string GroupId { get; set; } = "cryptojackpot-consumer-group";
    public string UserEventsTopic { get; set; } = "user-events";
    public bool EnableAutoCommit { get; set; } = true;
    public int SessionTimeoutMs { get; set; } = 45000;
}

