namespace CryptoJackpotService.Models.Configuration;

public record BrevoConfiguration
{
    public required string ApiKey { get; init; }
    public required string Email { get; init; }
    public required string SenderName { get; init; }
    public required string BaseUrl { get; init; }
}