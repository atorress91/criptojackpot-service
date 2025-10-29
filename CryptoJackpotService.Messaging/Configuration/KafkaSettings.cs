namespace CryptoJackpotService.Messaging.Configuration;

public class KafkaSettings
{
    public string BootstrapServers { get; set; } = "localhost:9092";
    public string GroupId { get; set; } = "cryptojackpot-consumer-group";
    public string UserCreatedTopic { get; set; } = "user-created-events";
    public string PasswordResetTopic { get; set; } = "password-reset-events";
    public bool EnableAutoCommit { get; set; } = true;
    public int SessionTimeoutMs { get; set; } = 45000;
}

