using CryptoJackpotService.Data.Database;
using CryptoJackpotService.Data.Database.Models;
using CryptoJackpotService.Data.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace CryptoJackpotService.Data.Repositories;

public class RoleRepository(CryptoJackpotDbContext context) : BaseRepository(context), IRoleRepository
{
    public Task<List<Role>> GetAllRoles()
        => Context.Roles.AsNoTracking().ToListAsync();
}

