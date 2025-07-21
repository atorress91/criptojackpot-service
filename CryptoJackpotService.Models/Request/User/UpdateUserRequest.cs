namespace CryptoJackpotService.Models.Request.User;

public class UpdateUserRequest
{
    public string Name { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Phone { get; set; } = null!;
    public string Password { get; set; } = null!;
}