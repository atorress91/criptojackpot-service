using CryptoJackpotService.Models.Constants;
using CryptoJackpotService.Models.Request.Prize;
using CryptoJackpotService.Models.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace CryptoJackpotService.Core.Validators;

public class UpdatePrizeRequestValidator : LocalizedValidator<UpdatePrizeRequest>
{
    public UpdatePrizeRequestValidator(IStringLocalizer<ISharedResource> localizer) : base(localizer)
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage(Localizer[ValidationMessages.Required]);

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage(Localizer[ValidationMessages.Required])
            .MaximumLength(200).WithMessage(_ => Localizer[ValidationMessages.MaxLength, 200]);

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage(Localizer[ValidationMessages.Required])
            .MaximumLength(2000).WithMessage(_ => Localizer[ValidationMessages.MaxLength, 2000]);

        RuleFor(x => x.MainImageUrl)
            .NotEmpty().WithMessage(Localizer[ValidationMessages.Required]);
    }
}