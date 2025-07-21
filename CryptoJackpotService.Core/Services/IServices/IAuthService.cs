using CryptoJackpotService.Models.DTO.User;
using CryptoJackpotService.Models.Request.Auth;
using CryptoJackpotService.Models.Responses;

namespace CryptoJackpotService.Core.Services.IServices;

public interface IAuthService
{
    Task<ResultResponse<UserDto?>> AuthenticateAsync(AuthenticateRequest request);
}