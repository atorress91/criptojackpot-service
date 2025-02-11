using CryptoJackpotService.Models.DTO;
using CryptoJackpotService.Models.Request;

namespace CryptoJackpotService.Core.Services.IServices;

public interface IAuthService
{
    Task<UserDto?> AuthenticateAsync(AuthenticateRequest request);
}