using AutoMapper;
using CryptoJackpotService.Core.Services.IServices;
using CryptoJackpotService.Data.Repositories.IRepositories;
using CryptoJackpotService.Models.Configuration;
using CryptoJackpotService.Models.DTO;
using CryptoJackpotService.Models.Request;
using CryptoJackpotService.Utility.Extensions;
using Microsoft.Extensions.Options;

namespace CryptoJackpotService.Core.Services;

public class AuthService(IMapper mapper, IUserRepository userRepository, IOptions<ApplicationConfiguration> appSettings)
    : BaseService(mapper), IAuthService
{
    public async Task<UserDto?> AuthenticateAsync(AuthenticateRequest request)
    {
        var user = await userRepository.GetUserAsyncByEmail(request.Email);
        
        if (user == null || !CommonExtensions.ValidatePass(user.Password, request.Password))
            return null;
        
        if (!user.Status)
            return null;

        var userDto = Mapper.Map<UserDto>(user);
        userDto.Token = user.Id.ToString().GenerateJwtToken(appSettings);

        return userDto;
    }
}