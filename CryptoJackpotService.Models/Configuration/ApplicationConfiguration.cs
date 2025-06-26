namespace CryptoJackpotService.Models.Configuration;

public class ApplicationConfiguration
{
    public ConnectionStrings? ConnectionStrings { get; init; }
    public JwtConfig? JwtSettings { get; init; }
    public BrevoConfiguration? BrevoConfiguration { get; init; }
    
    public DigitalOceanSettings? DigitalOceanSettings { get; init; }
}