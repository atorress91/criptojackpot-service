using System;
using System.Threading.Tasks;
using CryptoJackpotService.Data.Database;
using CryptoJackpotService.Data.Database.Models;
using CryptoJackpotService.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace CryptoJackpotService.Tests.Repositories;

public class UserRepositoryTests
{
    private static CryptoJackpotDbContext CreateInMemoryDbContext(string dbName)
    {
        var options = new DbContextOptionsBuilder<CryptoJackpotDbContext>()
            .UseInMemoryDatabase(databaseName: dbName)
            .EnableSensitiveDataLogging()
            .Options;
        return new CryptoJackpotService.Tests.Infrastructure.TestCryptoJackpotDbContext(options);
    }

    private static async Task<(CryptoJackpotDbContext ctx, Role role, Country country)> SeedBasicsAsync(string dbName)
    {
        var ctx = CreateInMemoryDbContext(dbName);
        var now = DateTime.UtcNow;
        var role = new Role { Name = "User", CreatedAt = now, UpdatedAt = now };
        var country = new Country { Name = "Testland", CreatedAt = now, UpdatedAt = now };
        ctx.Roles.Add(role);
        ctx.Countries.Add(country);
        await ctx.SaveChangesAsync();
        return (ctx, role, country);
    }

    [Fact]
    public async Task GetUserAsyncByEmail_returns_user_when_exists()
    {
        var (ctx, role, country) = await SeedBasicsAsync(nameof(GetUserAsyncByEmail_returns_user_when_exists));
        var repo = new UserRepository(ctx);
        var now = DateTime.UtcNow;
        var user = new User
        {
            Name = "Alice",
            LastName = "Tester",
            Email = "alice@example.com",
            Password = "secret",
            CountryId = country.Id,
            StatePlace = "State",
            City = "City",
            Status = true,
            RoleId = role.Id,
            CreatedAt = now,
            UpdatedAt = now
        };
        ctx.Users.Add(user);
        await ctx.SaveChangesAsync();

        var found = await repo.GetUserAsyncByEmail("alice@example.com");

        Assert.NotNull(found);
        Assert.Equal(user.Email, found!.Email);
    }

    [Fact]
    public async Task CreateUserAsync_sets_timestamps_and_persists()
    {
        var (ctx, role, country) = await SeedBasicsAsync(nameof(CreateUserAsync_sets_timestamps_and_persists));
        var repo = new UserRepository(ctx);
        var newUser = new User
        {
            Name = "Bob",
            LastName = "Tester",
            Email = "bob@example.com",
            Password = "secret",
            CountryId = country.Id,
            StatePlace = "State",
            City = "City",
            Status = true,
            RoleId = role.Id
        };

        var created = await repo.CreateUserAsync(newUser);

        Assert.True(created.Id > 0);
        Assert.NotEqual(default, created.CreatedAt);
        Assert.NotEqual(default, created.UpdatedAt);
        Assert.Equal("bob@example.com", created.Email);
    }
}
