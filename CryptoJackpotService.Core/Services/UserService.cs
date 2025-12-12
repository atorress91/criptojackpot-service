using AutoMapper;
using CryptoJackpotService.Core.Services.IServices;
using CryptoJackpotService.Data.Database.Models;
using CryptoJackpotService.Data.Repositories.IRepositories;
using CryptoJackpotService.Messaging.Events;
using CryptoJackpotService.Messaging.Producers;
using CryptoJackpotService.Models.Constants;
using CryptoJackpotService.Models.DTO.User;
using CryptoJackpotService.Models.Enums;
using CryptoJackpotService.Models.Request.User;
using CryptoJackpotService.Models.Resources;
using CryptoJackpotService.Models.Responses;
using CryptoJackpotService.Utility.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace CryptoJackpotService.Core.Services;

public class UserService(
    IMapper mapper,
    IUserRepository userRepository,
    ILogger<UserService> logger,
    IStringLocalizer<ISharedResource> localizer,
    IDigitalOceanStorageService digitalOceanStorageService,
    IConfiguration configuration,
    IEventProducer? eventProducer = null)
    : BaseService(mapper), IUserService
{
    

    public async Task<ResultResponse<UserDto>> CreateUserAsync(CreateUserRequest request)
    {
        User? referrerUser = null;
        if (!string.IsNullOrEmpty(request.ReferralCode))
        {
            referrerUser = await userRepository.GetUserBySecurityCodeAsync(request.ReferralCode);
            if (referrerUser is null)
            {
                return ResultResponse<UserDto>.Failure(ErrorType.BadRequest,
                    localizer[ValidationMessages.InvalidReferralCode]);
            }
        }

        var user = Mapper.Map<User>(request);
        user.SecurityCode = Guid.NewGuid().ToString();
        user.Status = false;
        user.Password = request.Password.EncryptPass();

        user = await userRepository.CreateUserAsync(user);

        // Publicar evento de creación de usuario
        if (eventProducer != null)
        {
            var userCreatedEvent = new UserCreatedEvent(
                userId: user.Id,
                email: user.Email,
                name: user.Name,
                lastName: user.LastName,
                securityCode: user.SecurityCode!,
                referrerId: referrerUser?.Id,
                referralCode: request.ReferralCode
            );

            var topic = configuration.GetValue<string>("Kafka:UserCreatedTopic") ?? "user-created-events";
            
            // Fire-and-forget: no esperamos la respuesta
            _ = Task.Run(async () =>
            {
                try
                {
                    await eventProducer.PublishAsync(userCreatedEvent, topic);
                    logger.LogInformation("UserCreatedEvent published for user {UserId}", user.Id);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Failed to publish UserCreatedEvent for user {UserId}", user.Id);
                }
            });
        }
        
        var userDto = Mapper.Map<UserDto>(user);
        return ResultResponse<UserDto>.Created(userDto);
    }

    public async Task<ResultResponse<UserDto>> UpdateImageProfile(UpdateImageProfileRequest request)
    {
        var user = await userRepository.GetUserAsyncById(request.UserId);

        if (user is null)
            return ResultResponse<UserDto>.Failure(ErrorType.NotFound, localizer[ValidationMessages.UserNotExists]);

        user.ImagePath = request.ImageUrl;
        var updatedUser = await userRepository.UpdateUserAsync(user);
        var userDto = Mapper.Map<UserDto>(updatedUser);

        if (userDto.ImagePath != null)
            userDto.ImagePath = digitalOceanStorageService.GetPresignedUrl(userDto.ImagePath);

        return ResultResponse<UserDto>.Ok(userDto);
    }

    public async Task<ResultResponse<UserDto>> GenerateNewSecurityCode(long userId)
    {
        var user = await userRepository.GetUserAsyncById(userId);

        if (user is null)
            return ResultResponse<UserDto>.Failure(ErrorType.NotFound, localizer[ValidationMessages.UserNotExists]);

        user.SecurityCode = Guid.NewGuid().ToString();
        var updatedUser = await userRepository.UpdateUserAsync(user);
        var userDto = Mapper.Map<UserDto>(updatedUser);

        if (userDto.ImagePath != null)
            userDto.ImagePath = digitalOceanStorageService.GetPresignedUrl(userDto.ImagePath);

        return ResultResponse<UserDto>.Ok(userDto);
    }

    public async Task<ResultResponse<UserDto>> UpdateUserAsync(long userId, UpdateUserRequest request)
    {
        var user = await userRepository.GetUserAsyncById(userId);

        if (user is null)
            return ResultResponse<UserDto>.Failure(ErrorType.NotFound, localizer[ValidationMessages.UserNotExists]);

        user.Name = request.Name;
        user.LastName = request.LastName;
        user.Phone = request.Phone;

        if (!string.IsNullOrWhiteSpace(request.Password))
            user.Password = request.Password.EncryptPass();

        var updatedUser = await userRepository.UpdateUserAsync(user);
        var userDto = Mapper.Map<UserDto>(updatedUser);
        
        if (userDto.ImagePath != null)
            userDto.ImagePath = digitalOceanStorageService.GetPresignedUrl(userDto.ImagePath);
        
        return ResultResponse<UserDto>.Ok(userDto);
    }

    public async Task<ResultResponse<UserDto>> UpdatePasswordAsync(UpdatePasswordRequest request)
    {
        var user = await userRepository.GetUserAsyncById(request.UserId);

        if (user is null)
            return ResultResponse<UserDto>.Failure(ErrorType.NotFound, localizer[ValidationMessages.UserNotExists]);
        
        var currentPasswordEncrypted = request.CurrentPassword.EncryptPass();
        if (user.Password != currentPasswordEncrypted)
            return ResultResponse<UserDto>.Failure(ErrorType.BadRequest, localizer[ValidationMessages.InvalidCurrentPassword]);
        
        user.Password = request.NewPassword.EncryptPass();
        var updatedUser = await userRepository.UpdateUserAsync(user);
        var userDto = Mapper.Map<UserDto>(updatedUser);

        if (userDto.ImagePath != null)
            userDto.ImagePath = digitalOceanStorageService.GetPresignedUrl(userDto.ImagePath);

        return ResultResponse<UserDto>.Ok(userDto);
    }

    public async Task<ResultResponse<UserDto>> GetUserAsyncById(long userId)
    {
        var user = await userRepository.GetUserAsyncById(userId);
        if (user is null)
            return ResultResponse<UserDto>.Failure(ErrorType.NotFound, localizer[ValidationMessages.UserNotExists]);
        
        var userDto = Mapper.Map<UserDto>(user);
        if (userDto.ImagePath != null)
            userDto.ImagePath = digitalOceanStorageService.GetPresignedUrl(userDto.ImagePath);
        
        return ResultResponse<UserDto>.Ok(userDto);
    }

    public async Task<ResultResponse<IEnumerable<UserDto>>> GetAllUsersAsync(long excludeUserId)
    {
        var users = await userRepository.GetAllUsersAsync(excludeUserId);
        if(users is null)
            return ResultResponse<IEnumerable<UserDto>>.Failure(ErrorType.NotFound, localizer[ValidationMessages.UserNotExists]);
        
        var userDtos = Mapper.Map<IEnumerable<UserDto>>(users);
        var enumerable = userDtos.ToList();
        
        return ResultResponse<IEnumerable<UserDto>>.Ok(enumerable);
    }

    public async Task<ResultResponse<string>> RequestPasswordResetAsync(RequestPasswordResetRequest request)
    {
        var user = await userRepository.GetUserAsyncByEmail(request.Email);
        
        if (user is null)
            return ResultResponse<string>.Failure(ErrorType.NotFound, localizer[ValidationMessages.UserNotExists]);
        
        var random = new Random();
        var securityCode = random.Next(100000, 999999).ToString();
        
        user.SecurityCode = securityCode;
        user.PasswordResetCodeExpiration = DateTime.UtcNow.AddMinutes(15);
        
        await userRepository.UpdateUserAsync(user);

        // Publicar evento de reset de contraseña
        if (eventProducer != null)
        {
            var passwordResetEvent = new PasswordResetRequestedEvent(
                userId: user.Id,
                email: user.Email,
                name: user.Name,
                lastName: user.LastName,
                securityCode: securityCode
            );

            var topic = configuration.GetValue<string>("Kafka:PasswordResetTopic") ?? "password-reset-events";
            
            // Fire-and-forget: no esperamos la respuesta
            _ = Task.Run(async () =>
            {
                try
                {
                    await eventProducer.PublishAsync(passwordResetEvent, topic);
                    logger.LogInformation("PasswordResetRequestedEvent published for user {UserId}", user.Id);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Failed to publish PasswordResetRequestedEvent for user {UserId}", user.Id);
                }
            });
        }

        return ResultResponse<string>.Ok(localizer[ValidationMessages.PasswordResetEmailSent]);
    }

    public async Task<ResultResponse<UserDto>> ResetPasswordWithCodeAsync(ResetPasswordWithCodeRequest request)
    {
        var user = await userRepository.GetUserAsyncByEmail(request.Email);
        
        if (user is null)
            return ResultResponse<UserDto>.Failure(ErrorType.NotFound, localizer[ValidationMessages.UserNotExists]);
        
        if (string.IsNullOrEmpty(user.SecurityCode) || 
            user.SecurityCode != request.SecurityCode ||
            user.PasswordResetCodeExpiration == null || 
            user.PasswordResetCodeExpiration < DateTime.UtcNow)
            return ResultResponse<UserDto>.Failure(ErrorType.BadRequest, 
                localizer[ValidationMessages.InvalidOrExpiredSecurityCode]);

        if (request.NewPassword != request.ConfirmPassword)
            return ResultResponse<UserDto>.Failure(ErrorType.BadRequest, 
                localizer[ValidationMessages.PasswordsDoNotMatch]);
        
        user.Password = request.NewPassword.EncryptPass();
        user.SecurityCode = null;
        user.PasswordResetCodeExpiration = null;

        var updatedUser = await userRepository.UpdateUserAsync(user);
        var userDto = Mapper.Map<UserDto>(updatedUser);

        if (userDto.ImagePath != null)
            userDto.ImagePath = digitalOceanStorageService.GetPresignedUrl(userDto.ImagePath);

        return ResultResponse<UserDto>.Ok(userDto);
    }
}