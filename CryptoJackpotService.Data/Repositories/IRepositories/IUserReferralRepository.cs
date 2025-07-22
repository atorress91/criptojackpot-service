using CryptoJackpotService.Data.Database.Custom;
using CryptoJackpotService.Data.Database.Models;

namespace CryptoJackpotService.Data.Repositories.IRepositories;

public interface IUserReferralRepository
{
    Task<UserReferral?> CheckIfUserIsReferred(long userId);
    Task<UserReferral> CreateUserReferralAsync(UserReferral userReferral);
    Task<IEnumerable<UserReferral>> GetAllReferralsByUserId(long userId);
    Task<IEnumerable<UserReferralWithStats>> GetReferralStatsAsync(long userId);
}