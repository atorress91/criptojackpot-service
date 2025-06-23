using AutoMapper;
using CryptoJackpotService.Core.Services.IServices;
using CryptoJackpotService.Data.Database.Models;
using CryptoJackpotService.Data.Repositories.IRepositories;
using CryptoJackpotService.Models.DTO;
using CryptoJackpotService.Models.Request;
using CryptoJackpotService.Models.Resources;
using CryptoJackpotService.Utility.Extensions;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace CryptoJackpotService.Core.Services;

public class UserService : BaseService, IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IBrevoService _brevoService;
    private readonly ILogger<UserService> _logger;
    private readonly IMapper _mapper;
    private readonly IStringLocalizer<SharedResource> _localizer;

    public UserService(
        IMapper mapper,
        IUserRepository userRepository,
        IBrevoService brevoService,
        ILogger<UserService> logger,
        IStringLocalizer<SharedResource> localizer) : base(mapper)
    {
        _mapper = mapper;
        _userRepository = userRepository;
        _brevoService = brevoService;
        _logger = logger;
        _localizer = localizer;
    }

    public async Task<UserDto?> CreateUserAsync(CreateUserRequest request)
    {
        var existingUser = await _userRepository.GetUserAsyncByEmail(request.Email);
        if (existingUser != null)
        {
            throw new InvalidOperationException(_localizer[ValidationMessages.EmailAlreadyExists]);
        }

        var user = _mapper.Map<User>(request);
        user.SecurityCode = Guid.NewGuid().ToString();
        user.Status = false;
        user.Password = request.Password.EncryptPass();

        try
        {
            user = await _userRepository.CreateUserAsync(user);

            var emailData = new Dictionary<string, string>
            {
                { "name", user.Name },
                { "lastName", user.LastName },
                { "token", user.SecurityCode! },
                { "user-email", user.Email },
                { "subject", _localizer["EmailConfirmationSubject"] ?? "Confirm your email" }
            };

            var emailResult = await _brevoService.SendEmailConfirmationAsync(emailData);
            if (!emailResult.Success)
            {
                _logger.LogWarning("Failed to send confirmation email: {Error}", emailResult.Message);
            }

            return _mapper.Map<UserDto>(user);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create user");
            return null;
        }
    }
}