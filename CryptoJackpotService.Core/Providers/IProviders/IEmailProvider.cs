using CryptoJackpotService.Models.Responses;

namespace CryptoJackpotService.Core.Providers.IProviders;

public interface IEmailProvider
{
    Task<ResultResponse<string>> SendEmailAsync(
        string recipientEmail,
        string subject,
        string htmlContent);
}

