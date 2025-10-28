using CryptoJackpotService.Data.Database.Models;

namespace CryptoJackpotService.Data.Repositories.IRepositories;

public interface IRoleRepository
{
    Task<List<Role>> GetAllRoles();
}

