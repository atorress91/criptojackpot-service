﻿using CryptoJackpotService.Models.Constants;
using CryptoJackpotService.Models.Request.User;
using CryptoJackpotService.Models.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace CryptoJackpotService.Core.Validators;

public class UpdateImageProfileRequestValidator : LocalizedValidator<UpdateImageProfileRequest>
{
    public UpdateImageProfileRequestValidator(IStringLocalizer<ISharedResource> localizer) : base(localizer)
    {
        RuleFor(x => x.UserId)
            .NotNull().WithMessage(localizer[ValidationMessages.Required]);

        RuleFor(x => x.ImageUrl)
            .NotEmpty().WithMessage(Localizer[ValidationMessages.Required]);
    }
}