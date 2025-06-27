namespace CryptoJackpotService.Models.Constants;

public static class Constants
{
    public const string BaseUrl = "https://criptojackpot.com";
    public const string UrlConfirmEmail = "user_confirm_email";
    public const string ConfirmEmailTemplate = "email-confirmation.html";
    public static readonly HashSet<string> AllowedExtensions = new(StringComparer.OrdinalIgnoreCase)
    {
        ".jpg", ".jpeg", ".png", ".webp"
    };
}