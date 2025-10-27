namespace CryptoJackpotService.Models.Request.User;

public class UpdatePasswordRequest
{
    public long UserId { get; set; }
    public string CurrentPassword { get; set; } = null!;
    public string NewPassword { get; set; } = null!;
    public string ConfirmPassword { get; set; } = null!;
}

