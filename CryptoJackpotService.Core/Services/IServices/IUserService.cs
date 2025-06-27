using CryptoJackpotService.Models.DTO;
using CryptoJackpotService.Models.Request;
using CryptoJackpotService.Models.Responses;

namespace CryptoJackpotService.Core.Services.IServices;

public interface IUserService
{
    Task<ResultResponse<UserDto>> CreateUserAsync(CreateUserRequest request);
    Task<ResultResponse<UserDto>> UpdateImageProfile(UpdateImageProfileRequest request);
}