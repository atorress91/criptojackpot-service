using System.Collections.Immutable;

namespace CryptoJackpotService.Models.Constants;

public static class Constants
{
    public const string BaseUrl = "https://criptojackpot.com";
    public const string UrlConfirmEmail = "user_confirm_email";
    public const string ConfirmEmailTemplate = "email-confirmation.html";
    public const string PasswordResetTemplate = "password-reset.html";
    public static readonly ImmutableHashSet<string> AllowedExtensions = 
        ImmutableHashSet.Create(StringComparer.OrdinalIgnoreCase, ".jpg", ".jpeg", ".png", ".webp");
}