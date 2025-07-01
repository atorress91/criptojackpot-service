using CryptoJackpotService.Data.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace CryptoJackpotService.Data.Database;

public class CryptoJackpotDbContext(DbContextOptions<CryptoJackpotDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Role> Roles { get; set; } = null!;
    public DbSet<Permission> Permissions { get; set; } = null!;
    public DbSet<RolePermission> RolePermissions { get; set; } = null!;
    public DbSet<Country> Countries { get; set; } = null!;
    public DbSet<Lottery> Lotteries { get; set; } = null!;
    public DbSet<LotteryNumber> LotteryNumbers { get; set; } = null!;
    public DbSet<Prize> Prizes { get; set; } = null!;
    public DbSet<PrizeImage> PrizeImages { get; set; } = null!;
    public DbSet<PrizeTier> PrizeTiers { get; set; } = null!;
    public DbSet<Ticket> Tickets { get; set; } = null!;
    public DbSet<Winner> Winners { get; set; } = null!;
    public DbSet<Invoice> Invoices { get; set; } = null!;
    public DbSet<InvoiceDetail> InvoiceDetails { get; set; } = null!;
    public DbSet<Transaction> Transactions { get; set; } = null!;
    public DbSet<UserReferral> UserReferrals { get; set; } = null!;
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSnakeCaseNamingConvention();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CryptoJackpotDbContext).Assembly);
    }
}