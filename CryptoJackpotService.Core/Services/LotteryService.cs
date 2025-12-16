using AutoMapper;
using CryptoJackpotService.Core.Services.IServices;
using CryptoJackpotService.Data.Database.Models;
using CryptoJackpotService.Data.Repositories.IRepositories;
using CryptoJackpotService.Models.Configuration;
using CryptoJackpotService.Models.Constants;
using CryptoJackpotService.Models.DTO.Lottery;
using CryptoJackpotService.Models.Enums;
using CryptoJackpotService.Models.Request.Lottery;
using CryptoJackpotService.Models.Request.Pagination;
using CryptoJackpotService.Models.Resources;
using CryptoJackpotService.Models.Responses;
using CryptoJackpotService.Utility.Extensions;
using Microsoft.Extensions.Localization;

namespace CryptoJackpotService.Core.Services;

public class LotteryService(
    IMapper mapper,
    IStringLocalizer<ISharedResource> localizer,
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

    public async Task<ResultResponsePaged<LotteryDto>> GetAllLotteriesAsync(PaginationRequest pagination)
    {
        var paginationModel = Mapper.Map<Pagination>(pagination);
        var lotteries = await lotteryRepository.GetAllLotteriesAsync(paginationModel);

        var lotteryDtos = Mapper.Map<IEnumerable<LotteryDto>>(lotteries.Items);

        return ResultResponsePaged<LotteryDto>.Ok(
            lotteryDtos,
            lotteries.PageNumber,
            lotteries.PageSize,
            lotteries.TotalItems,
            lotteries.TotalPages);
    }

    public async Task<ResultResponse<LotteryDto>> GetLotteryByIdAsync(Guid lotteryId)
    {
        var lottery = await lotteryRepository.GetLotteryAsync(lotteryId);

        if (lottery is null)
            return ResultResponse<LotteryDto>.Failure(ErrorType.NotFound, localizer[ValidationMessages.LotteryNotFound]);

        var lotteryDto = Mapper.Map<LotteryDto>(lottery);

        return ResultResponse<LotteryDto>.Ok(lotteryDto);
    }

    public async Task<ResultResponse<LotteryDto>> UpdateLotteryAsync(Guid lotteryId, UpdateLotteryRequest request)
    {
        var lottery = await lotteryRepository.GetLotteryAsync(lotteryId);

        if (lottery is null)
            return ResultResponse<LotteryDto>.Failure(ErrorType.NotFound, localizer[ValidationMessages.LotteryNotFound]);

        lottery.Title = request.Title;
        lottery.Description = request.Description;
        lottery.MinNumber = request.MinNumber;
        lottery.MaxNumber = request.MaxNumber;
        lottery.TotalSeries = request.TotalSeries;
        lottery.TicketPrice = request.TicketPrice;
        lottery.MaxTickets = request.MaxTickets;
        lottery.StartDate = request.StartDate;
        lottery.EndDate = request.EndDate;
        lottery.Status = (LotteryStatus)request.Status;
        lottery.Type = (LotteryType)request.Type;
        lottery.Terms = request.Terms;
        lottery.HasAgeRestriction = request.HasAgeRestriction;
        lottery.MinimumAge = request.MinimumAge;
        lottery.RestrictedCountries = request.RestrictedCountries;

        lottery = await lotteryRepository.UpdateAsync(lottery);

        var lotteryDto = Mapper.Map<LotteryDto>(lottery);

        return ResultResponse<LotteryDto>.Ok(lotteryDto);
    }

    public async Task<ResultResponse<LotteryDto>> DeleteLotteryAsync(Guid lotteryId)
    {
        var lottery = await lotteryRepository.GetLotteryAsync(lotteryId);

        if (lottery is null)
            return ResultResponse<LotteryDto>.Failure(ErrorType.NotFound, localizer[ValidationMessages.LotteryNotFound]);

        lottery = await lotteryRepository.DeleteLotteryAsync(lottery);
        var lotteryDto = Mapper.Map<LotteryDto>(lottery);

        return ResultResponse<LotteryDto>.Ok(lotteryDto);
    }
}