using CryptoJackpotService.Data.Database.Models;

namespace CryptoJackpotService.Data.Repositories.IRepositories;

public interface IUserRepository
{ 
    Task<User> CreateUserAsync(User user);
    Task<User?> GetUserAsyncById(int id);
    Task<User?> GetUserAsyncByEmail(string email);
}