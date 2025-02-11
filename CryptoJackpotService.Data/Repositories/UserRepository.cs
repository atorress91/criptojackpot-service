using CryptoJackpotService.Data.Database;
using CryptoJackpotService.Data.Database.Models;
using CryptoJackpotService.Data.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace CryptoJackpotService.Data.Repositories;

public class UserRepository(CryptoJackpotDbContext context) : BaseRepository(context), IUserRepository
{
    public async Task<User> CreateUserAsync(User user)
    {
        var today = DateTime.Now;
        user.CreatedAt = today;
        user.UpdatedAt = today;

        await Context.Users.AddAsync(user);
        await Context.SaveChangesAsync();

        return await Context.Users
            .Include(u => u.Role)
            .Include(u => u.Country)
            .FirstAsync(u => u.Id == user.Id);
    }

    public async Task<User?> GetUserAsyncById(int id)
        => await Context.Users.FindAsync(id);

    public async Task<User?> GetUserAsyncByEmail(string email)
        => await Context.Users.FirstOrDefaultAsync(u => u.Email == email);
}