namespace CryptoJackpotService.Messaging.Events;

public record UserCreatedEvent(
    long UserId,
    string Email,
    string Name,
    string LastName,
    string SecurityCode,
    long? ReferrerId,
    string? ReferralCode,
    DateTime OccurredAt)
{
    public UserCreatedEvent(
        long userId,
        string email,
        string name,
        string lastName,
        string securityCode,
        long? referrerId = null,
        string? referralCode = null)
        : this(userId, email, name, lastName, securityCode, referrerId, referralCode, DateTime.UtcNow)
    {
    }
}