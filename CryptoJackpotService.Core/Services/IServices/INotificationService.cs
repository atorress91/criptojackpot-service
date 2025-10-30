namespace CryptoJackpotService.Core.Services.IServices;

public interface INotificationService
{
    Task SendEmailConfirmationAsync(
        long userId,
        string email,
        string name,
        string lastName,
        string token);

    Task SendPasswordResetEmailAsync(
        string email,
        string name,
        string lastName,
        string securityCode);

    Task SendReferralNotificationAsync(
        string referrerEmail,
        string referrerName,
        string referrerLastName,
        string referredName,
        string referredLastName,
        string referralCode);
}