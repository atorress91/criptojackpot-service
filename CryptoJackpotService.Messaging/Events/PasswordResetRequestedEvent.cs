using Newtonsoft.Json;

namespace CryptoJackpotService.Messaging.Events;

[method: JsonConstructor]
public record PasswordResetRequestedEvent(
    long UserId,
    string Email,
    string Name,
    string LastName,
    string SecurityCode,
    DateTime OccurredAt)
{
    public PasswordResetRequestedEvent(
        long userId,
        string email,
        string name,
        string lastName,
        string securityCode)
        : this(userId, email, name, lastName, securityCode, DateTime.UtcNow)
    {
    }
}

