using CryptoJackpotService.Models.Request;
using CryptoJackpotService.Models.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace CryptoJackpotService.Core.Validators;

public class AuthenticatedRequestValidator : LocalizedValidator<AuthenticateRequest>
{
    public AuthenticatedRequestValidator(IStringLocalizer<SharedResource> localizer) : base(localizer)
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage(Localizer[ValidationMessages.Required])
            .EmailAddress().WithMessage(Localizer[ValidationMessages.InvalidEmail]);

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage(Localizer[ValidationMessages.Required])
            .MinimumLength(8).WithMessage(x => Localizer[ValidationMessages.MinLength, 8])
            .MaximumLength(16).WithMessage(x => Localizer[ValidationMessages.MaxLength, 16])
            .Matches("[A-Z]").WithMessage(Localizer[ValidationMessages.PasswordUppercase])
            .Matches("[a-z]").WithMessage(Localizer[ValidationMessages.PasswordLowercase])
            .Matches("[0-9]").WithMessage(Localizer[ValidationMessages.PasswordNumber])
            .Matches("[^a-zA-Z0-9]").WithMessage(Localizer[ValidationMessages.PasswordSpecialChar]);
    }
}