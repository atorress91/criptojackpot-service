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

    public async Task<User?> GetUserAsyncById(long id)
        => await Context.Users.FindAsync(id);

    public async Task<User?> GetUserAsyncByEmail(string email)
        => await Context.Users.FirstOrDefaultAsync(u => u.Email == email);

    public async Task<User> UpdateUserAsync(User user)
    {
        var today = DateTime.Now;
        user.UpdatedAt = today;
        Context.Users.Update(user);
        await Context.SaveChangesAsync();
        return user;
    }

    public async Task<User?> GetUserBySecurityCodeAsync(string securityCode)
        => await Context.Users.FirstOrDefaultAsync(u => u.SecurityCode == securityCode);

    public async Task<IEnumerable<User>?> GetAllUsersAsync(long excludeUserId)
        => await Context.Users.Where(u => u.Id != excludeUserId).ToListAsync();
}