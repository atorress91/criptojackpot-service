using CryptoJackpotService.Core.Providers.IProviders;
using CryptoJackpotService.Models.Configuration;
using CryptoJackpotService.Models.Enums;
using CryptoJackpotService.Models.Responses;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using sib_api_v3_sdk.Api;
using sib_api_v3_sdk.Client;
using sib_api_v3_sdk.Model;

namespace CryptoJackpotService.Core.Providers;

public class BrevoProvider : IEmailProvider
{
    private readonly ILogger<BrevoProvider> _logger;
    private readonly ApplicationConfiguration _appConfig;
    private readonly TransactionalEmailsApi _emailApi;

    public BrevoProvider(
        IOptions<ApplicationConfiguration> appConfig,
        ILogger<BrevoProvider> logger)
    {
        _appConfig = appConfig.Value ?? throw new ArgumentNullException(nameof(appConfig));
        _logger = logger;

        Configuration.Default.AddApiKey("api-key", _appConfig.BrevoConfiguration!.ApiKey);
        _emailApi = new TransactionalEmailsApi();
    }

    public async Task<ResultResponse<string>> SendEmailAsync(
        string recipientEmail,
        string subject,
        string htmlContent)
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
            _logger.LogInformation("Email sent successfully via Brevo: {MessageId}", result.MessageId);
            return ResultResponse<string>.Ok(result.MessageId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send email via Brevo to {Email}", recipientEmail);
            return ResultResponse<string>.Failure(ErrorType.Unexpected, ex.Message);
        }
    }
}

