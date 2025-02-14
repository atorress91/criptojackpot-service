using CryptoJackpotService.Models.Responses;

namespace CryptoJackpotService.Core.Services.IServices;

public interface IBrevoService
{
    Task<Result<string>> SendEmailConfirmationAsync(Dictionary<string, string> data);
}