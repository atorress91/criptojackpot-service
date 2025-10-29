using CryptoJackpotService.Core.Providers.IProviders;
using CryptoJackpotService.Core.Services.IServices;
using CryptoJackpotService.Models.Configuration;
using CryptoJackpotService.Models.Constants;
using CryptoJackpotService.Models.Resources;
using CryptoJackpotService.Utility.Extensions;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CryptoJackpotService.Core.Services;

public class NotificationService(
    IEmailProvider emailProvider,
    IEmailTemplateProvider templateProvider,
    IStringLocalizer<ISharedResource> localizer,
    IOptions<ApplicationConfiguration> appConfig,
    ILogger<NotificationService> logger)
    : INotificationService
{
    private readonly ApplicationConfiguration _appConfig = appConfig.Value;

    public async Task SendEmailConfirmationAsync(
        long userId,
        string email,
        string name,
        string lastName,
        string token)
    {
        var templateResult = await templateProvider.GetTemplateAsync(Constants.ConfirmEmailTemplate);
        if (!templateResult.Success)
        {
            logger.LogError("Template not found: {Message}", templateResult.Message);
            throw new Exception($"Template not found: {templateResult.Message}");
        }

        var url = $"{_appConfig.BrevoConfiguration!.BaseUrl}{Constants.UrlConfirmEmail}/{token}";
        var fullName = $"{name} {lastName}";
        var subject = localizer["EmailConfirmationSubject"];

        var templateData = new Dictionary<string, string>
        {
            ["{0}"] = fullName,
            ["{1}"] = DateTime.Now.ToString("MM/dd/yyyy"),
            ["{2}"] = url
        };

        var body = templateResult.Data!.ReplaceHtml(templateData);

        var result = await emailProvider.SendEmailAsync(email, subject, body);

        if (!result.Success)
        {
            logger.LogWarning("Failed to send confirmation email for user {UserId}: {Message}", userId, result.Message);
            throw new Exception($"Failed to send email: {result.Message}");
        }

        logger.LogInformation("Email confirmation sent successfully for user {UserId}", userId);
    }

    public async Task SendPasswordResetEmailAsync(
        string email,
        string name,
        string lastName,
        string securityCode)
    {
        var templateResult = await templateProvider.GetTemplateAsync(Constants.PasswordResetTemplate);
        if (!templateResult.Success)
        {
            logger.LogError("Template not found: {Message}", templateResult.Message);
            throw new Exception($"Template not found: {templateResult.Message}");
        }

        var fullName = $"{name} {lastName}";
        var subject = localizer["PasswordResetSubject"];

        var templateData = new Dictionary<string, string>
        {
            ["{0}"] = fullName,
            ["{1}"] = securityCode,
            ["{2}"] = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss")
        };

        var body = templateResult.Data!.ReplaceHtml(templateData);

        var result = await emailProvider.SendEmailAsync(email, subject, body);

        if (!result.Success)
        {
            logger.LogWarning("Failed to send password reset email to {Email}: {Message}", email, result.Message);
            throw new Exception($"Failed to send email: {result.Message}");
        }

        logger.LogInformation("Password reset email sent successfully to {Email}", email);
    }
}

