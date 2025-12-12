using CryptoJackpotService.Models.DTO.Lottery;
using CryptoJackpotService.Models.Request.Lottery;
using CryptoJackpotService.Models.Responses;

namespace CryptoJackpotService.Core.Services.IServices;

public interface ILotteryService
{
    Task<ResultResponse<LotteryDto>> CreateLotteryAsync(CreateLotteryRequest request);
}