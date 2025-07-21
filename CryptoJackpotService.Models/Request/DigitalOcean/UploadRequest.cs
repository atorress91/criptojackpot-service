namespace CryptoJackpotService.Models.Request;

public class UploadRequest
{
    public long UserId { get; init; } = 0;
    public string FileName { get; set; } = null!;
    public string ContentType { get; set; } = null!;
    public int? ExpirationMinutes { get; set; }
}