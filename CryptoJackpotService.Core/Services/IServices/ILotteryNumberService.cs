using CryptoJackpotService.Models.DTO.LotteryNumber;
using CryptoJackpotService.Models.Responses;

namespace CryptoJackpotService.Core.Services.IServices;

public interface ILotteryNumberService
{
    Task<ResultResponse<List<LotteryNumberDto>>> GetAvailableNumbersAsync(Guid lotteryId, int count = 10);
    Task<ResultResponse<bool>> IsNumberAvailableAsync(Guid lotteryId, int number, int series);
    Task<ResultResponse<List<LotteryNumberDto>>> ReserveNumbersAsync(Guid lotteryId, Guid ticketId, List<int> numbers, int series);
    Task<ResultResponse<bool>> ReleaseNumbersAsync(Guid ticketId);
    Task<ResultResponse<LotteryNumberStatsDto>> GetNumberStatsAsync(Guid lotteryId);
}