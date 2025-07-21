using CryptoJackpotService.Models.Constants;
using CryptoJackpotService.Models.Request.User;
using CryptoJackpotService.Models.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace CryptoJackpotService.Core.Validators;

public class CreateUserRequestValidator : LocalizedValidator<CreateUserRequest>
{
    public CreateUserRequestValidator(IStringLocalizer<SharedResource> localizer) : base(localizer)
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage(Localizer[ValidationMessages.Required]);

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage(Localizer[ValidationMessages.Required]);

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage(Localizer[ValidationMessages.Required])
            .EmailAddress().WithMessage(Localizer[ValidationMessages.InvalidEmail]);

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage(Localizer[ValidationMessages.Required])
            .MinimumLength(8).WithMessage(_ => Localizer[ValidationMessages.MinLength, 8])
            .MaximumLength(16).WithMessage(_ => Localizer[ValidationMessages.MaxLength, 16])
            .Matches("[A-Z]").WithMessage(Localizer[ValidationMessages.PasswordUppercase])
            .Matches("[a-z]").WithMessage(Localizer[ValidationMessages.PasswordLowercase])
            .Matches("[0-9]").WithMessage(Localizer[ValidationMessages.PasswordNumber])
            .Matches("[^a-zA-Z0-9]").WithMessage(Localizer[ValidationMessages.PasswordSpecialChar]);

        RuleFor(x => x.RoleId)
            .NotEmpty().WithMessage(Localizer[ValidationMessages.Required])
            .GreaterThan(0).WithMessage(Localizer[ValidationMessages.Required]);

        RuleFor(x => x.CountryId)
            .NotEmpty().WithMessage(Localizer[ValidationMessages.Required])
            .GreaterThan(0).WithMessage(Localizer[ValidationMessages.Required]);

        RuleFor(x => x.StatePlace)
            .NotEmpty().WithMessage(Localizer[ValidationMessages.Required])
            .MaximumLength(100).WithMessage(_ => Localizer[ValidationMessages.MaxLength, 100]);

        RuleFor(x => x.City)
            .NotEmpty().WithMessage(Localizer[ValidationMessages.Required])
            .MaximumLength(100).WithMessage(_ => Localizer[ValidationMessages.MaxLength, 100]);

        RuleFor(x => x.Address)
            .MaximumLength(200).WithMessage(_ => Localizer[ValidationMessages.MaxLength, 200])
            .When(x => !string.IsNullOrEmpty(x.Address));

        RuleFor(x => x.ImagePath)
            .MaximumLength(500).WithMessage(_ => Localizer[ValidationMessages.MaxLength, 500])
            .When(x => !string.IsNullOrEmpty(x.ImagePath));

        RuleFor(x => x.GoogleAccessToken)
            .MaximumLength(2000).WithMessage(_ => Localizer[ValidationMessages.MaxLength, 2000])
            .When(x => !string.IsNullOrEmpty(x.GoogleAccessToken));

        RuleFor(x => x.GoogleRefreshToken)
            .MaximumLength(2000).WithMessage(_ => Localizer[ValidationMessages.MaxLength, 2000])
            .When(x => !string.IsNullOrEmpty(x.GoogleRefreshToken));
    }
}