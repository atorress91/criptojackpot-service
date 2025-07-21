namespace CryptoJackpotService.Models.Request;

public class UpdateUserRequest
{
    public string Name { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Phone { get; set; } = null!;
    public string Password { get; set; } = null!;
}