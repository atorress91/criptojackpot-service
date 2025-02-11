using AutoMapper;
using CryptoJackpotService.Core.Services.IServices;
using CryptoJackpotService.Data.Database.Models;
using CryptoJackpotService.Data.Repositories.IRepositories;
using CryptoJackpotService.Models.DTO;
using CryptoJackpotService.Models.Request;
using CryptoJackpotService.Utility.Extensions;

namespace CryptoJackpotService.Core.Services;

public class UserService(IMapper mapper, IUserRepository userRepository) : BaseService(mapper), IUserService
{
    public async Task<UserDto?> CreateUserAsync(CreateUserRequest request)
    {
        var passwordEncrypt = request.Password.EncryptPass();
        request.Password = passwordEncrypt;

        var user = Mapper.Map<User>(request);
        user = await userRepository.CreateUserAsync(user);

        return Mapper.Map<UserDto>(user);
    }
}