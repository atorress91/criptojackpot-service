using CryptoJackpotService.Models.DTO.Prize;
using CryptoJackpotService.Models.Request.Pagination;
using CryptoJackpotService.Models.Request.Prize;
using CryptoJackpotService.Models.Responses;

namespace CryptoJackpotService.Core.Services.IServices;

public interface IPrizeService
{
    Task<ResultResponse<PrizeDto>> GetPrizeAsyncById(Guid prizeId);
    Task<ResultResponse<PrizeDto>> CreatePrizeAsync(CreatePrizeRequest request);
    Task<ResultResponsePaged<PrizeDto>> GetAllPrizesAsync(PaginationRequest pagination);
    Task<ResultResponse<PrizeDto>> UpdatePrizeAsync(UpdatePrizeRequest request);
    Task<ResultResponse<PrizeDto>> DeletePrizeAsync(Guid prizeId);
}