namespace CryptoJackpotService.Models.Request.User;

public class UpdateImageProfileRequest
{
    public long UserId { get; set; }
    public string ImageUrl { get; set; } = null!;
}