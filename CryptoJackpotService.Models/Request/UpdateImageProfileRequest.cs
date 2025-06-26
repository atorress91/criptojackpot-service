namespace CryptoJackpotService.Models.Request;

public class UpdateImageProfileRequest
{
    public int UserId { get; set; }
    public string ImageUrl { get; set; } = null!;
}