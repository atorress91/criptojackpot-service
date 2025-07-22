using CryptoJackpotService.Models.DTO.UserReferral;
using CryptoJackpotService.Models.Request.Referral;
using CryptoJackpotService.Models.Responses;

namespace CryptoJackpotService.Core.Services.IServices;

public interface IUserReferralService
{
    Task<ResultResponse<UserReferralDto>> CreateUserReferralAsync(UserReferralRequest request);
    Task<ResultResponse<UserReferralStatsDto>> GetReferralStatsAsync(long userId);
}