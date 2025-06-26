using CryptoJackpotService.Models.DTO;
using CryptoJackpotService.Models.Request;

namespace CryptoJackpotService.Core.Services.IServices;

public interface IUserService
{
    Task<UserDto> CreateUserAsync(CreateUserRequest request);
    Task<UserDto> UpdateImageProfile(UpdateImageProfileRequest request);
}