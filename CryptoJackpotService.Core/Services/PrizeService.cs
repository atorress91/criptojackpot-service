using AutoMapper;
using CryptoJackpotService.Core.Services.IServices;
using CryptoJackpotService.Data.Database.Models;
using CryptoJackpotService.Data.Repositories.IRepositories;
using CryptoJackpotService.Models.Configuration;
using CryptoJackpotService.Models.DTO.Prize;
using CryptoJackpotService.Models.Request.Pagination;
using CryptoJackpotService.Models.Request.Prize;
using CryptoJackpotService.Models.Responses;

namespace CryptoJackpotService.Core.Services;

public class PrizeService(
    IMapper mapper,
    IPrizeRepository prizeRepository)
    : BaseService(mapper), IPrizeService
{
    public async Task<ResultResponse<PrizeDto>> CreatePrizeAsync(CreatePrizeRequest request)
    {
        var prize = Mapper.Map<Prize>(request);

        prize = await prizeRepository.CreatePrizeAsync(prize);

        var prizeDto = Mapper.Map<PrizeDto>(prize);

        return ResultResponse<PrizeDto>.Ok(prizeDto);
    }

    public async Task<ResultResponsePaged<PrizeDto>> GetAllPrizesAsync(PaginationRequest pagination)
    {
        var paginationModel = Mapper.Map<Pagination>(pagination);
        var prizes = await prizeRepository.GetAllPrizesAsync(paginationModel);

        var prizeDtos = Mapper.Map<IEnumerable<PrizeDto>>(prizes.Items);

        return ResultResponsePaged<PrizeDto>.Ok(
            prizeDtos,
            prizes.PageNumber,
            prizes.PageSize,
            prizes.TotalItems,
            prizes.TotalPages);
    }
}