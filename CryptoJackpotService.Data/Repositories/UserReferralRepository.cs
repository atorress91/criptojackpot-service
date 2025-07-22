using CryptoJackpotService.Data.Database;
using CryptoJackpotService.Data.Database.Custom;
using CryptoJackpotService.Data.Database.Models;
using CryptoJackpotService.Data.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace CryptoJackpotService.Data.Repositories;

public class UserReferralRepository(CryptoJackpotDbContext context) : BaseRepository(context), IUserReferralRepository
{
    public async Task<UserReferral?> CheckIfUserIsReferred(long userId)
        => await Context.UserReferrals.FirstOrDefaultAsync(x => x.ReferredId == userId);

    public async Task<UserReferral> CreateUserReferralAsync(UserReferral userReferral)
    {
        await Context.UserReferrals.AddAsync(userReferral);
        await Context.SaveChangesAsync();

        return await Context.UserReferrals
            .Include(ur => ur.Referrer)
            .Include(ur => ur.Referred)
            .FirstAsync(ur => ur.Id == userReferral.Id);
    }

    public async Task<IEnumerable<UserReferral>> GetAllReferralsByUserId(long userId)
        => await Context.UserReferrals.Where(x => x.ReferrerId == userId).ToListAsync();

    public async Task<IEnumerable<UserReferralWithStats>> GetReferralStatsAsync(long userId)
        => await Context.UserReferrals
            .Where(ur => ur.ReferrerId == userId)
            .Select(ur => new UserReferralWithStats
            {
                UsedSecurityCode = ur.UsedSecurityCode,
                RegisterDate = ur.Referred.CreatedAt,
                FullName = ur.Referred.Name + " " + ur.Referred.LastName,
                Email = ur.Referred.Email,
            })
            .ToListAsync();
}