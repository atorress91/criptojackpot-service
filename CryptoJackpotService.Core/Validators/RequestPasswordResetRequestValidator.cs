using CryptoJackpotService.Models.Constants;
using CryptoJackpotService.Models.Request.User;
using CryptoJackpotService.Models.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace CryptoJackpotService.Core.Validators;

public class RequestPasswordResetRequestValidator : LocalizedValidator<RequestPasswordResetRequest>
{
    public RequestPasswordResetRequestValidator(IStringLocalizer<ISharedResource> localizer) : base(localizer)
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage(Localizer[ValidationMessages.Required])
            .EmailAddress().WithMessage(Localizer[ValidationMessages.InvalidEmail]);
    }
}

