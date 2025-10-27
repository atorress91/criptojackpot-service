namespace CryptoJackpotService.Models.Request.User;

public class ResetPasswordWithCodeRequest
{
    public string Email { get; set; } = null!;
    public string SecurityCode { get; set; } = null!;
    public string NewPassword { get; set; } = null!;
    public string ConfirmPassword { get; set; } = null!;
}

