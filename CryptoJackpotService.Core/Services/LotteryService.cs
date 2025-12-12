using AutoMapper;
using CryptoJackpotService.Core.Services.IServices;
using CryptoJackpotService.Data.Database.Models;
using CryptoJackpotService.Data.Repositories.IRepositories;
using CryptoJackpotService.Models.DTO.Lottery;
using CryptoJackpotService.Models.Request.Lottery;
using CryptoJackpotService.Models.Responses;
using CryptoJackpotService.Utility.Extensions;

namespace CryptoJackpotService.Core.Services;

public class LotteryService(
    IMapper mapper,
    ILotteryRepository lotteryRepository)
    : BaseService(mapper), ILotteryService
{
    public async Task<ResultResponse<LotteryDto>> CreateLotteryAsync(CreateLotteryRequest request)
    {
        var lottery = Mapper.Map<Lottery>(request);
        lottery.Id = Guid.NewGuid();
        lottery.LotteryNo = CommonExtensions.GenerateLotteryNumber();
        lottery.SoldTickets = 0;

        lottery = await lotteryRepository.CreateAsync(lottery);

        var lotteryDto = Mapper.Map<LotteryDto>(lottery);
        return ResultResponse<LotteryDto>.Created(lotteryDto);
    }
}