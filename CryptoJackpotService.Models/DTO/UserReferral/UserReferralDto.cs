namespace CryptoJackpotService.Models.DTO.UserReferral;

public class UserReferralDto
{
    public long Id { get; set; }
    public long ReferrerId { get; set; }
    public long ReferredId { get; set; }
    public string UsedSecurityCode { get; set; } = string.Empty;
    
    public DateTime? RegisterDate { get; set; }
    public string? FullName { get; set; } 
    public string? Email { get; set; } 
}