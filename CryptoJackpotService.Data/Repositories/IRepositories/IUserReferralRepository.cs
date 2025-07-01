using CryptoJackpotService.Data.Database.Models;

namespace CryptoJackpotService.Data.Repositories.IRepositories;

public interface IUserReferralRepository
{
    Task<UserReferral?> CheckIfUserIsReferred(long userId);
    Task<UserReferral> CreateUserReferralAsync(UserReferral userReferral);
}