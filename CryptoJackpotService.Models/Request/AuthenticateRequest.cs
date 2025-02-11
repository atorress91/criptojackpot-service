namespace CryptoJackpotService.Models.Request;

public class AuthenticateRequest
{
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
}