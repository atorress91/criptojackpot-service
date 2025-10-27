using CryptoJackpotService.Models.Responses;

namespace CryptoJackpotService.Core.Services.IServices;

public interface IBrevoService
{
    Task<ResultResponse<string>> SendEmailConfirmationAsync(Dictionary<string, string> data);
    Task<ResultResponse<string>> SendPasswordResetEmailAsync(Dictionary<string, string> data);
}