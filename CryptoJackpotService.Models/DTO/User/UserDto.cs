using CryptoJackpotService.Models.DTO.Country;
using CryptoJackpotService.Models.DTO.Role;

namespace CryptoJackpotService.Models.DTO.User;

public class UserDto
{
    public long Id { get; set; }
    public string Name { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? Identification { get; set; }
    public string? Phone { get; set; }
    public string StatePlace { get; set; } = null!;
    public string City { get; set; } = null!;
    public string? Address { get; set; }
    public bool Status { get; set; }
    public string? SecurityCode { get; set; } 
    public string? ImagePath { get; set; }
    public string? Token { get; set; }
    
    public RoleDto Role { get; set; } = null!;
    public CountryDto Country { get; set; } = null!;
}