using CryptoJackpotService.Models.Constants;
using CryptoJackpotService.Models.Request.User;
using CryptoJackpotService.Models.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace CryptoJackpotService.Core.Validators;

public class UpdatePasswordRequestValidator : LocalizedValidator<UpdatePasswordRequest>
{
    public UpdatePasswordRequestValidator(IStringLocalizer<ISharedResource> localizer) : base(localizer)
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage(Localizer[ValidationMessages.Required])
            .GreaterThan(0).WithMessage(Localizer[ValidationMessages.Required]);

        RuleFor(x => x.CurrentPassword)
            .NotEmpty().WithMessage(Localizer[ValidationMessages.Required]);

        RuleFor(x => x.NewPassword)
            .NotEmpty().WithMessage(Localizer[ValidationMessages.Required])
            .MinimumLength(8).WithMessage(_ => Localizer[ValidationMessages.MinLength, 8])
            .MaximumLength(16).WithMessage(_ => Localizer[ValidationMessages.MaxLength, 16])
            .Matches("[A-Z]").WithMessage(Localizer[ValidationMessages.PasswordUppercase])
            .Matches("[a-z]").WithMessage(Localizer[ValidationMessages.PasswordLowercase])
            .Matches("[0-9]").WithMessage(Localizer[ValidationMessages.PasswordNumber])
            .Matches("[^a-zA-Z0-9]").WithMessage(Localizer[ValidationMessages.PasswordSpecialChar])
            .NotEqual(x => x.CurrentPassword).WithMessage(Localizer[ValidationMessages.NewPasswordMustBeDifferent]);

        RuleFor(x => x.ConfirmPassword)
            .NotEmpty().WithMessage(Localizer[ValidationMessages.Required])
            .Equal(x => x.NewPassword).WithMessage(Localizer[ValidationMessages.PasswordsDoNotMatch]);
    }
}

