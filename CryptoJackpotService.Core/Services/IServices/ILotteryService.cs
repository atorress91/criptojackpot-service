using CryptoJackpotService.Models.DTO.Lottery;
using CryptoJackpotService.Models.Request.Lottery;
using CryptoJackpotService.Models.Request.Pagination;
using CryptoJackpotService.Models.Responses;

namespace CryptoJackpotService.Core.Services.IServices;

public interface ILotteryService
{
    Task<ResultResponse<LotteryDto>> CreateLotteryAsync(CreateLotteryRequest request);
    Task<ResultResponsePaged<LotteryDto>> GetAllLotteriesAsync(PaginationRequest pagination);
    Task<ResultResponse<LotteryDto>> GetLotteryByIdAsync(Guid lotteryId);
    Task<ResultResponse<LotteryDto>> UpdateLotteryAsync(Guid lotteryId, UpdateLotteryRequest request);
    Task<ResultResponse<LotteryDto>> DeleteLotteryAsync(Guid lotteryId);
}