namespace CryptoJackpotService.Models.DTO.UserReferral;

public class UserReferralStatsDto
{
    public int? TotalEarnings { get; set; }
    public int? LastMonthEarnings { get; set; }  
    public IEnumerable<UserReferralDto> Referrals { get; set; } = null!;
}