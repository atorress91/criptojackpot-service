namespace CryptoJackpotService.Models.DTO.UserReferral;

public class UserReferralDto
{
    public long Id { get; set; }
    public long ReferrerId { get; set; }
    public long ReferredId { get; set; }
    public string UsedSecurityCode { get; set; } = string.Empty;
}