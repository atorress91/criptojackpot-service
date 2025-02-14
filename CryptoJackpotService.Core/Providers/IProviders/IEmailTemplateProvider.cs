using CryptoJackpotService.Models.Responses;

namespace CryptoJackpotService.Core.Providers.IProviders;

public interface IEmailTemplateProvider
{
    Task<Result<string>> GetTemplateAsync(string templateName);
}