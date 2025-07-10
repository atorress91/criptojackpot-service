using AutoMapper;
using CryptoJackpotService.Core.Services.IServices;
using CryptoJackpotService.Data.Database.Models;
using CryptoJackpotService.Data.Repositories.IRepositories;
using CryptoJackpotService.Models.Constants;
using CryptoJackpotService.Models.DTO;
using CryptoJackpotService.Models.Enums;
using CryptoJackpotService.Models.Request;
using CryptoJackpotService.Models.Resources;
using CryptoJackpotService.Models.Responses;
using CryptoJackpotService.Utility.Extensions;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace CryptoJackpotService.Core.Services;

public class UserService : BaseService, IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IBrevoService _brevoService;
    private readonly ILogger<UserService> _logger;
    private readonly IMapper _mapper;
    private readonly IStringLocalizer<SharedResource> _localizer;
    private readonly IDigitalOceanStorageService _digitalOceanStorageService;
    private readonly IUserReferralService _userReferralService;

    public UserService(
        IMapper mapper,
        IUserRepository userRepository,
        IBrevoService brevoService,
        ILogger<UserService> logger,
        IStringLocalizer<SharedResource> localizer,
        IDigitalOceanStorageService digitalOceanStorageService,
        IUserReferralService userReferralService) : base(mapper)
    {
        _mapper = mapper;
        _userRepository = userRepository;
        _brevoService = brevoService;
        _logger = logger;
        _localizer = localizer;
        _digitalOceanStorageService = digitalOceanStorageService;
        _userReferralService = userReferralService;
    }

    public async Task<ResultResponse<UserDto>> CreateUserAsync(CreateUserRequest request)
    {
        var existingUser = await _userRepository.GetUserAsyncByEmail(request.Email);
        if (existingUser != null)
            return ResultResponse<UserDto>.Failure(ErrorType.Conflict,
                _localizer[ValidationMessages.EmailAlreadyExists]);

        User? referrerUser = null;
        if (!string.IsNullOrEmpty(request.ReferralCode))
        {
            referrerUser = await _userRepository.GetUserBySecurityCodeAsync(request.ReferralCode);
            if (referrerUser is null)
            {
                return ResultResponse<UserDto>.Failure(ErrorType.BadRequest,
                    _localizer[ValidationMessages.InvalidReferralCode]);
            }
        }

        var user = _mapper.Map<User>(request);
        user.SecurityCode = Guid.NewGuid().ToString();
        user.Status = false;
        user.Password = request.Password.EncryptPass();

        user = await _userRepository.CreateUserAsync(user);

        if (referrerUser != null)
        {
            await _userReferralService.CreateUserReferralAsync(new UserReferralRequest
                { ReferredId = user.Id, ReferrerId = referrerUser.Id, ReferralCode = request.ReferralCode });
        }

        var emailData = new Dictionary<string, string>
        {
            { "name", user.Name },
            { "lastName", user.LastName },
            { "token", user.SecurityCode! },
            { "user-email", user.Email },
            { "subject", _localizer["EmailConfirmationSubject"] }
        };

        var emailResult = await _brevoService.SendEmailConfirmationAsync(emailData);
        if (!emailResult.Success)
        {
            _logger.LogWarning("Failed to send confirmation email: {Error}", emailResult.Message);
        }

        var userDto = _mapper.Map<UserDto>(user);
        return ResultResponse<UserDto>.Ok(userDto);
    }

    public async Task<ResultResponse<UserDto>> UpdateImageProfile(UpdateImageProfileRequest request)
    {
        var user = await _userRepository.GetUserAsyncById(request.UserId);

        if (user is null)
            return ResultResponse<UserDto>.Failure(ErrorType.NotFound, _localizer[ValidationMessages.UserNotExists]);

        user.ImagePath = request.ImageUrl;
        var updatedUser = await _userRepository.UpdateUserAsync(user);
        var userDto = _mapper.Map<UserDto>(updatedUser);

        if (userDto.ImagePath != null)
            userDto.ImagePath = _digitalOceanStorageService.GetPresignedUrl(userDto.ImagePath);

        return ResultResponse<UserDto>.Ok(userDto);
    }

    public async Task<ResultResponse<UserDto>> GenerateNewSecurityCode(long userId)
    {
        var user = await _userRepository.GetUserAsyncById(userId);
        
        if (user is null)
            return ResultResponse<UserDto>.Failure(ErrorType.NotFound, _localizer[ValidationMessages.UserNotExists]);
        
        user.SecurityCode = Guid.NewGuid().ToString();
        var updatedUser = await _userRepository.UpdateUserAsync(user);
        var userDto = _mapper.Map<UserDto>(updatedUser);
        
        if (userDto.ImagePath != null)
            userDto.ImagePath = _digitalOceanStorageService.GetPresignedUrl(userDto.ImagePath);
        
        return ResultResponse<UserDto>.Ok(userDto);
    }
}