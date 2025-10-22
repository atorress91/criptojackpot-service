using System.Threading.Tasks;
using AutoMapper;
using CryptoJackpotService.Core.Mapper;
using CryptoJackpotService.Core.Services;
using CryptoJackpotService.Core.Services.IServices;
using CryptoJackpotService.Data.Database.Models;
using CryptoJackpotService.Data.Repositories.IRepositories;
using CryptoJackpotService.Models.Constants;
using CryptoJackpotService.Models.Request.User;
using CryptoJackpotService.Models.Responses;
using CryptoJackpotService.Models.Enums;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace CryptoJackpotService.Tests.Services;

public class UserServiceTests
{
    private static IMapper CreateMapper()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile(new MapperProfile()));
        return config.CreateMapper();
    }

    private static IStringLocalizer<CryptoJackpotService.Models.Resources.ISharedResource> CreateLocalizer()
    {
        var mock = new Mock<IStringLocalizer<CryptoJackpotService.Models.Resources.ISharedResource>>();
        mock.Setup(l => l[It.IsAny<string>()])
            .Returns((string key) => new LocalizedString(key, key));
        return mock.Object;
    }

    private static UserService CreateService(
        Mock<IUserRepository> userRepoMock,
        Mock<IDigitalOceanStorageService>? storageMock = null)
    {
        var mapper = CreateMapper();
        var brevoMock = new Mock<IBrevoService>();
        var loggerMock = new Mock<ILogger<UserService>>();
        var localizer = CreateLocalizer();
        var referralMock = new Mock<IUserReferralService>();
        storageMock ??= new Mock<IDigitalOceanStorageService>();

        return new UserService(
            mapper,
            userRepoMock.Object,
            brevoMock.Object,
            loggerMock.Object,
            localizer,
            storageMock.Object,
            referralMock.Object);
    }

    [Fact]
    public async Task UpdateImageProfile_returns_NotFound_when_user_does_not_exist()
    {
        // Arrange
        var userRepo = new Mock<IUserRepository>();
        userRepo.Setup(r => r.GetUserAsyncById(It.IsAny<long>()))
            .ReturnsAsync((User?)null);
        var service = CreateService(userRepo);
        var request = new UpdateImageProfileRequest { UserId = 123, ImageUrl = "https://img" };

        // Act
        var result = await service.UpdateImageProfile(request);

        // Assert
        Assert.False(result.Success);
        Assert.Equal(ErrorType.NotFound, result.ErrorType);
        Assert.Equal(ValidationMessages.UserNotExists, result.Message);
        userRepo.Verify(r => r.UpdateUserAsync(It.IsAny<User>()), Times.Never);
    }

    [Fact]
    public async Task UpdateImageProfile_updates_image_and_returns_presigned_url()
    {
        // Arrange
        var existing = new User
        {
            Id = 10,
            Name = "A",
            LastName = "B",
            Email = "a@b.com",
            Password = "p",
            CountryId = 1,
            StatePlace = "S",
            City = "C",
            Status = true,
            RoleId = 1
        };

        var userRepo = new Mock<IUserRepository>();
        userRepo.Setup(r => r.GetUserAsyncById(existing.Id))
            .ReturnsAsync(existing);
        userRepo.Setup(r => r.UpdateUserAsync(It.IsAny<User>()))
            .ReturnsAsync((User u) => u);

        var storage = new Mock<IDigitalOceanStorageService>();
        storage.Setup(s => s.GetPresignedUrl("https://original/img.png"))
            .Returns("https://presigned/img.png");

        var service = CreateService(userRepo, storage);
        var request = new UpdateImageProfileRequest { UserId = existing.Id, ImageUrl = "https://original/img.png" };

        // Act
        var response = await service.UpdateImageProfile(request);

        // Assert
        Assert.True(response.Success);
        Assert.NotNull(response.Data);
        Assert.Equal(existing.Id, response.Data!.Id);
        Assert.Equal("https://presigned/img.png", response.Data.ImagePath);

        userRepo.Verify(r => r.UpdateUserAsync(It.Is<User>(u => u.ImagePath == request.ImageUrl)), Times.Once);
        storage.Verify(s => s.GetPresignedUrl(request.ImageUrl), Times.Once);
    }
}
