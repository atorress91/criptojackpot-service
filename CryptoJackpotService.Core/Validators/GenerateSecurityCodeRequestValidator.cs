using CryptoJackpotService.Models.Constants;
using CryptoJackpotService.Models.Request;
using CryptoJackpotService.Models.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace CryptoJackpotService.Core.Validators;

public class GenerateSecurityCodeRequestValidator : LocalizedValidator<GenerateSecurityCodeRequest>
{
    public GenerateSecurityCodeRequestValidator(IStringLocalizer<SharedResource> localizer):base(localizer)
    {
        RuleFor(x => x.UserId)
            .NotNull().WithMessage(localizer[ValidationMessages.Required]);
    }
}