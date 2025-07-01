using CryptoJackpotService.Data.Database.Models;

namespace CryptoJackpotService.Data.Repositories.IRepositories;

public interface IUserRepository
{ 
    Task<User> CreateUserAsync(User user);
    Task<User?> GetUserAsyncById(long id);
    Task<User?> GetUserAsyncByEmail(string email);
    Task<User> UpdateUserAsync(User user);
    Task<User?> GetUserBySecurityCodeAsync(string securityCode);
}