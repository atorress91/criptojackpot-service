namespace CryptoJackpotService.Models.Configuration;

public class ApplicationConfiguration
{
    public ConnectionStrings? ConnectionStrings { get; set; }
    public JwtConfig? JwtSettings { get; set; }
    public BrevoConfiguration? BrevoConfiguration { get; set; }
}