using CryptoJackpotService.Models.DTO.Prize;
using CryptoJackpotService.Models.Request.Prize;
using CryptoJackpotService.Models.Responses;

namespace CryptoJackpotService.Core.Services.IServices;

public interface IPrizeService
{
    Task<ResultResponse<PrizeDto>> CreatePrizeAsync(CreatePrizeRequest request);
}