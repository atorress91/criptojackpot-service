using CryptoJackpotService.Data.Database;
using CryptoJackpotService.Data.Database.Models;
using CryptoJackpotService.Data.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace CryptoJackpotService.Data.Repositories;

public class UserReferralRepository(CryptoJackpotDbContext context) : BaseRepository(context), IUserReferralRepository
{
    public async Task<UserReferral?> CheckIfUserIsReferred(long userId)
        => await Context.UserReferrals.FirstOrDefaultAsync(x => x.ReferredId == userId);
}