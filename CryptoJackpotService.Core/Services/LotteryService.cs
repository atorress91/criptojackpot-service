using AutoMapper;
using CryptoJackpotService.Core.Services.IServices;
using CryptoJackpotService.Data.Repositories.IRepositories;
namespace CryptoJackpotService.Core.Services;

public class LotteryService(
    IMapper mapper,
    ILotteryRepository lotteryRepository)
    : BaseService(mapper), ILotteryService
{
}