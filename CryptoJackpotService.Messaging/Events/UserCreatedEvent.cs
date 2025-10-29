using Newtonsoft.Json;

namespace CryptoJackpotService.Messaging.Events;

public record UserCreatedEvent
{
    [JsonConstructor]
    public UserCreatedEvent(
        long userId,
        string email,
        string name,
        string lastName,
        string securityCode,
        UserReferralInfo? referralInfo,
        DateTime occurredAt)
    {
        UserId = userId;
        Email = email;
        Name = name;
        LastName = lastName;
        SecurityCode = securityCode;
        ReferrerId = referralInfo?.ReferrerId;
        ReferralCode = referralInfo?.ReferralCode;
        OccurredAt = occurredAt;
    }

    public UserCreatedEvent(
        long userId,
        string email,
        string name,
        string lastName,
        string securityCode,
        long? referrerId = null,
        string? referralCode = null)
        : this(
            userId, 
            email, 
            name, 
            lastName, 
            securityCode, 
            referrerId.HasValue || referralCode != null 
                ? new UserReferralInfo(referrerId, referralCode) 
                : null,
            DateTime.UtcNow)
    {
    }

    public long UserId { get; init; }
    public string Email { get; init; }
    public string Name { get; init; }
    public string LastName { get; init; }
    public string SecurityCode { get; init; }
    public long? ReferrerId { get; init; }
    public string? ReferralCode { get; init; }
    public DateTime OccurredAt { get; init; }
}

public record UserReferralInfo(long? ReferrerId, string? ReferralCode);
