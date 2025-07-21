namespace CryptoJackpotService.Models.Request;

public class UserReferralRequest
{
    public long ReferrerId { get; set; }
    public long ReferredId { get; set; }
    public string? ReferralCode { get; set; }
}