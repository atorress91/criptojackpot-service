using AutoMapper;
using CryptoJackpotService.Core.Services.IServices;
using CryptoJackpotService.Data.Database.Models;
using CryptoJackpotService.Data.Repositories.IRepositories;
using CryptoJackpotService.Models.Configuration;
using CryptoJackpotService.Models.Constants;
using CryptoJackpotService.Models.DTO.Prize;
using CryptoJackpotService.Models.Enums;
using CryptoJackpotService.Models.Request.Pagination;
using CryptoJackpotService.Models.Request.Prize;
using CryptoJackpotService.Models.Resources;
using CryptoJackpotService.Models.Responses;
using Microsoft.Extensions.Localization;

namespace CryptoJackpotService.Core.Services;

public class PrizeService(
    IMapper mapper,
    IPrizeRepository prizeRepository,
    IStringLocalizer<ISharedResource> localizer)
    : BaseService(mapper), IPrizeService
{
    public async Task<ResultResponse<PrizeDto>> GetPrizeAsyncById(Guid prizeId)
    {
        var prize = await prizeRepository.GetPrizeAsync(prizeId);

        if (prize is null)
            return ResultResponse<PrizeDto>.Failure(ErrorType.NotFound, localizer[ValidationMessages.PrizeNotFound]);

        var prizeDto = Mapper.Map<PrizeDto>(prize);

        return ResultResponse<PrizeDto>.Ok(prizeDto);
    }
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

    public async Task<ResultResponse<PrizeDto>> UpdatePrizeAsync(UpdatePrizeRequest request)
    {
        var prize = await prizeRepository.GetPrizeAsync(request.Id);

        if (prize is null)
            return ResultResponse<PrizeDto>.Failure(ErrorType.NotFound, localizer[ValidationMessages.PrizeNotFound]);

        prize.Name = request.Name;
        prize.Description = request.Description;
        prize.EstimatedValue = request.EstimatedValue;
        prize.MainImageUrl = request.MainImageUrl;
        prize.Specifications = request.Specifications;
        prize.CashAlternative = request.CashAlternative;
        prize.IsDeliverable = request.IsDeliverable;
        prize.IsDigital = request.IsDigital;
        prize.AdditionalImages = request.AdditionalImageUrls
            .Select(url => new PrizeImage
            {
                ImageUrl = url
            })
            .ToList();

        prize = await prizeRepository.UpdatePrizeAsync(prize);

        var prizeDto = Mapper.Map<PrizeDto>(prize);

        return ResultResponse<PrizeDto>.Ok(prizeDto);
    }

    public async Task<ResultResponse<PrizeDto>> DeletePrizeAsync(Guid prizeId)
    {
        var prize = await prizeRepository.GetPrizeAsync(prizeId);

        if (prize is null)
            return ResultResponse<PrizeDto>.Failure(ErrorType.NotFound, localizer[ValidationMessages.PrizeNotFound]);

        prize = await prizeRepository.DeletePrizeAsync(prize);
        var prizeDto = Mapper.Map<PrizeDto>(prize);

        return ResultResponse<PrizeDto>.Ok(prizeDto);
    }
}