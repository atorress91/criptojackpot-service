namespace CryptoJackpotService.Models.DTO.UserReferral;

public class UserReferralStatsDto
{
    public int TotalEarnings { get; set; }
    public int LastMonthEarnings { get; set; }  
    public DateTime RegisterDate { get; set; }
    public string FullName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string UsedSecurityCode { get; set; } = null!; 
}