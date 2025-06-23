using CryptoJackpotService.Models.Responses;

namespace CryptoJackpotService.Core.Providers.IProviders;

public interface IEmailTemplateProvider
{
    Task<ResultResponse<string>> GetTemplateAsync(string templateName);
}