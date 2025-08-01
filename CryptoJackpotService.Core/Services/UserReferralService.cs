﻿using AutoMapper;
using CryptoJackpotService.Core.Services.IServices;
using CryptoJackpotService.Data.Database.Models;
using CryptoJackpotService.Data.Repositories.IRepositories;
using CryptoJackpotService.Models.Constants;
using CryptoJackpotService.Models.DTO.UserReferral;
using CryptoJackpotService.Models.Enums;
using CryptoJackpotService.Models.Request.Referral;
using CryptoJackpotService.Models.Resources;
using CryptoJackpotService.Models.Responses;
using Microsoft.Extensions.Localization;

namespace CryptoJackpotService.Core.Services;

public class UserReferralService : BaseService, IUserReferralService
{
    private readonly IUserReferralRepository _userReferralRepository;
    private readonly IMapper _mapper;
    private readonly IStringLocalizer<ISharedResource> _localizer;
    
    public UserReferralService(IUserReferralRepository userReferralRepository, IMapper mapper,
    IStringLocalizer<ISharedResource> localizer): base(mapper)
    {
        _mapper = mapper;
        _userReferralRepository = userReferralRepository;
        _localizer = localizer;
    }

    public async Task<ResultResponse<UserReferralDto>> CreateUserReferralAsync(UserReferralRequest request)
    {
        var existingReferral = await _userReferralRepository.CheckIfUserIsReferred(request.ReferredId);
        if (existingReferral != null)
            return ResultResponse<UserReferralDto>.Failure(ErrorType.Conflict,_localizer[ValidationMessages.AlreadyReferred]);

        var userReferral = _mapper.Map<UserReferral>(request);
        
        var result = await _userReferralRepository.CreateUserReferralAsync(userReferral);
        var userReferralDto = _mapper.Map<UserReferralDto>(result);
        return ResultResponse<UserReferralDto>.Ok(userReferralDto);
    }
    
    public async Task<ResultResponse<UserReferralStatsDto>> GetReferralStatsAsync(long userId)
    {
        var referrals = await _userReferralRepository.GetReferralStatsAsync(userId);
        
        var referralStatsDto = new UserReferralStatsDto
        {
            TotalEarnings = 0, 
            LastMonthEarnings = 0,
            Referrals = _mapper.Map<IEnumerable<UserReferralDto>>(referrals) 
        };
        
        return ResultResponse<UserReferralStatsDto>.Ok(referralStatsDto); 
    }
}