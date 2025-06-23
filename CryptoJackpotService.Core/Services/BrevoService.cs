using System.Net;
using CryptoJackpotService.Core.Providers.IProviders;
using CryptoJackpotService.Core.Services.IServices;
using CryptoJackpotService.Models.Configuration;
using CryptoJackpotService.Models.Constants;
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

    public async Task<ResultResponse<string>> SendEmailConfirmationAsync(Dictionary<string, string> data)
    {
        var templateResult = await _templateProvider.GetTemplateAsync(Constants.ConfirmEmailTemplate);
        if (!templateResult.Success)
        {
            return ResultResponse<string>.Failure(templateResult.Error!,HttpStatusCode.BadRequest);
        }

        var url = $"{_appConfig.BrevoConfiguration!.BaseUrl}{Constants.UrlConfirmEmail}/{data["token"]}";
        var fullName = $"{data["name"]} {data["lastName"]}";

        var templateData = new Dictionary<string, string>
        {
            ["{0}"] = fullName,
            ["{1}"] = DateTime.Now.ToString("MM/dd/yyyy"),
            ["{2}"] = url
        };

        var body = templateResult.Value!.ReplaceHtml(templateData);

        try
        {
            var email = new SendSmtpEmail
            {
                To = new List<SendSmtpEmailTo> { new(data["user-email"]) },
                Subject = data["subject"],
                HtmlContent = body,
                Sender = new SendSmtpEmailSender(_appConfig.BrevoConfiguration.SenderName, _appConfig.BrevoConfiguration.Email)
            };

            var result = await _emailApi.SendTransacEmailAsync(email);
            _logger.LogInformation("Email sent successfully: {MessageId}", result.MessageId);
            return ResultResponse<string>.Ok(result.MessageId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send email to {Email}", data["user-email"]);
            return ResultResponse<string>.Failure(ex.Message, HttpStatusCode.InternalServerError);
        }
    }
}