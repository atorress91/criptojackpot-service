using AutoMapper;
using CryptoJackpotService.Core.Services.IServices;
using CryptoJackpotService.Data.Repositories.IRepositories;
using CryptoJackpotService.Models.Configuration;
using CryptoJackpotService.Models.Constants;
using CryptoJackpotService.Models.DTO;
using CryptoJackpotService.Models.Enums;
using CryptoJackpotService.Models.Request;
using CryptoJackpotService.Models.Resources;
using CryptoJackpotService.Models.Responses;
using CryptoJackpotService.Utility.Extensions;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;

namespace CryptoJackpotService.Core.Services;

public class AuthService : BaseService, IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IOptions<ApplicationConfiguration> _appSettings;
    private readonly IStringLocalizer<SharedResource> _localizer;
    private readonly IDigitalOceanStorageService _digitalOceanStorageService;

    public AuthService(
        IMapper mapper,
        IUserRepository userRepository,
        IOptions<ApplicationConfiguration> appSettings,
        IStringLocalizer<SharedResource> localizer,
        IDigitalOceanStorageService digitalOceanStorageService)
        : base(mapper)
    {
        _userRepository = userRepository;
        _appSettings = appSettings;
        _localizer = localizer;
        _digitalOceanStorageService = digitalOceanStorageService;
    }

    public async Task<ResultResponse<UserDto?>> AuthenticateAsync(AuthenticateRequest request)
    {
        var user = await _userRepository.GetUserAsyncByEmail(request.Email);

        if (user == null || !CommonExtensions.ValidatePass(user.Password, request.Password))
            return ResultResponse<UserDto?>.Failure(ErrorType.Unauthorized,_localizer[ValidationMessages.InvalidCredentials]);

        if (!user.Status)
            return ResultResponse<UserDto?>.Failure(ErrorType.Forbidden,_localizer[ValidationMessages.UserNotVerified]);

        if (user.ImagePath is not null)
        { 
            user.ImagePath =  _digitalOceanStorageService.GetPresignedUrl(user.ImagePath);
        }
        
        var userDto = Mapper.Map<UserDto>(user);
        userDto.Token = user.Id.ToString().GenerateJwtToken(_appSettings);

        return ResultResponse<UserDto?>.Ok(userDto);
    }
}