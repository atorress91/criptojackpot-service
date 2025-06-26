namespace CryptoJackpotService.Models.Request;

public class UploadRequest
{
    public string FileName { get; set; } = null!;
    public string ContentType { get; set; } = null!;
    public int? ExpirationMinutes { get; set; }
}