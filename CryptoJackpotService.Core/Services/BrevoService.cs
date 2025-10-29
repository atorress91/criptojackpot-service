using CryptoJackpotService.Core.Providers.IProviders;
using CryptoJackpotService.Core.Services.IServices;
using CryptoJackpotService.Models.Configuration;
using CryptoJackpotService.Models.Constants;
using CryptoJackpotService.Models.Enums;
using CryptoJackpotService.Models.Responses;
using CryptoJackpotService.Utility.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using sib_api_v3_sdk.Api;
using sib_api_v3_sdk.Client;
using sib_api_v3_sdk.Model;

namespace CryptoJackpotService.Core.Services;

public class BrevoService : IBrevoService
{
    private readonly IEmailTemplateProvider _templateProvider;
    private readonly ILogger<BrevoService> _logger;
    private readonly ApplicationConfiguration _appConfig;
    private readonly TransactionalEmailsApi _emailApi;

    public BrevoService(
        IOptions<ApplicationConfiguration> appConfig,
        IEmailTemplateProvider templateProvider,
        ILogger<BrevoService> logger)
    {
        _appConfig = appConfig.Value ?? throw new ArgumentNullException(nameof(appConfig));
        _templateProvider = templateProvider;
        _logger = logger;

        Configuration.Default.AddApiKey("api-key", _appConfig.BrevoConfiguration!.ApiKey);
        _emailApi = new TransactionalEmailsApi();
    }

    private async Task<ResultResponse<string>> GetEmailTemplateAsync(string templateName)
    {
        var templateResult = await _templateProvider.GetTemplateAsync(templateName);
        return !templateResult.Success
            ? ResultResponse<string>.Failure(ErrorType.BadRequest, templateResult.Message!)
            : ResultResponse<string>.Ok(templateResult.Data!);
    }
    private static string GetFullName(Dictionary<string, string> data)
    {
        return $"{data["name"]} {data["lastName"]}";
    }
    private async Task<ResultResponse<string>> SendEmailAsync(
        string recipientEmail,
        string subject,
        string htmlContent,
        string logSuccessMessage,
        string logErrorMessage)
    {
        try
        {
            var email = new SendSmtpEmail
            {
                To = [new SendSmtpEmailTo(recipientEmail)],
                Subject = subject,
                HtmlContent = htmlContent,
                Sender = new SendSmtpEmailSender(
                    _appConfig.BrevoConfiguration.SenderName,
                    _appConfig.BrevoConfiguration.Email)
            };

            var result = await _emailApi.SendTransacEmailAsync(email);
            _logger.LogInformation(logSuccessMessage, result.MessageId);
            return ResultResponse<string>.Ok(result.MessageId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, logErrorMessage, recipientEmail);
            return ResultResponse<string>.Failure(ErrorType.Unexpected, ex.Message);
        }
    }

    public async Task<ResultResponse<string>> SendEmailConfirmationAsync(Dictionary<string, string> data)
    {
        var templateResult = await GetEmailTemplateAsync(Constants.ConfirmEmailTemplate);
        if (!templateResult.Success)
        {
            return templateResult;
        }

        var url = $"{_appConfig.BrevoConfiguration!.BaseUrl}{Constants.UrlConfirmEmail}/{data["token"]}";
        var fullName = GetFullName(data);

        var templateData = new Dictionary<string, string>
        {
            ["{0}"] = fullName,
            ["{1}"] = DateTime.Now.ToString("MM/dd/yyyy"),
            ["{2}"] = url
        };

        var body = templateResult.Data!.ReplaceHtml(templateData);

        return await SendEmailAsync(
            recipientEmail: data["user-email"],
            subject: data["subject"],
            htmlContent: body,
            logSuccessMessage: "Email confirmation sent successfully: {MessageId}",
            logErrorMessage: "Failed to send email confirmation to {Email}");
    }

    public async Task<ResultResponse<string>> SendPasswordResetEmailAsync(Dictionary<string, string> data)
    {
        var templateResult = await GetEmailTemplateAsync(Constants.PasswordResetTemplate);
        if (!templateResult.Success)
        {
            return templateResult;
        }

        var fullName = GetFullName(data);

        var templateData = new Dictionary<string, string>
        {
            ["{0}"] = fullName,
            ["{1}"] = data["securityCode"],
            ["{2}"] = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss")
        };

        var body = templateResult.Data!.ReplaceHtml(templateData);

        return await SendEmailAsync(
            recipientEmail: data["user-email"],
            subject: data["subject"],
            htmlContent: body,
            logSuccessMessage: "Password reset email sent successfully: {MessageId}",
            logErrorMessage: "Failed to send password reset email to {Email}");
    }
}