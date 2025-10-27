using CryptoJackpotService.Models.DTO.User;
using CryptoJackpotService.Models.Request.User;
using CryptoJackpotService.Models.Responses;

namespace CryptoJackpotService.Core.Services.IServices;

public interface IUserService
{
    Task<ResultResponse<UserDto>> CreateUserAsync(CreateUserRequest request);
    Task<ResultResponse<UserDto>> UpdateImageProfile(UpdateImageProfileRequest request);
    Task<ResultResponse<UserDto>> GenerateNewSecurityCode(long userId);
    Task<ResultResponse<UserDto>> UpdateUserAsync(long userId, UpdateUserRequest request);
    Task<ResultResponse<UserDto>> UpdatePasswordAsync(UpdatePasswordRequest request);
    Task<ResultResponse<UserDto>> GetUserAsyncById(long userId);
    Task<ResultResponse<IEnumerable<UserDto>>> GetAllUsersAsync(long excludeUserId);
}