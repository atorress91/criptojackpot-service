using CryptoJackpotService.Models.DTO;
using CryptoJackpotService.Models.Request;
using CryptoJackpotService.Models.Responses;

namespace CryptoJackpotService.Core.Services.IServices;

public interface IUserReferralService
{
    Task<ResultResponse<UserReferralDto>> CreateUserReferralAsync(UserReferralRequest request);
}