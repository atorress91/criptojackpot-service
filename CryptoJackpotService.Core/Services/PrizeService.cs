using AutoMapper;
using CryptoJackpotService.Core.Services.IServices;
using CryptoJackpotService.Data.Database.Models;
using CryptoJackpotService.Data.Repositories.IRepositories;
using CryptoJackpotService.Models.DTO.Prize;
using CryptoJackpotService.Models.Request.Prize;
using CryptoJackpotService.Models.Responses;

namespace CryptoJackpotService.Core.Services;

public class PrizeService(
    IMapper mapper,
    IPrizeRepository prizeRepository)
    : BaseService(mapper), IPrizeService
{
    private readonly IMapper _mapper = mapper;

    public async Task<ResultResponse<PrizeDto>> CreatePrizeAsync(CreatePrizeRequest request)
    {
        var prize = _mapper.Map<Prize>(request);
        
        prize = await prizeRepository.CreatePrizeAsync(prize);

        var prizeDto = _mapper.Map<PrizeDto>(prize);

        return ResultResponse<PrizeDto>.Ok(prizeDto);
    }
}