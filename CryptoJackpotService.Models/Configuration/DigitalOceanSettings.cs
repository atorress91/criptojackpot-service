namespace CryptoJackpotService.Models.Configuration;

public class DigitalOceanSettings
{
    public string AccessKey { get; set; } = string.Empty;
    public string SecretKey { get; set; } = string.Empty;
    public string Region { get; set; } = string.Empty;
    public string Endpoint { get; set; } = string.Empty;
    public string BucketName { get; set; } = string.Empty;
}