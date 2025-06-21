using CryptoJackpotService.Models.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace CryptoJackpotService.Core.Validators;

public abstract class LocalizedValidator<T> : AbstractValidator<T>
{
    protected readonly IStringLocalizer<SharedResource> Localizer;

    protected LocalizedValidator(IStringLocalizer<SharedResource> localizer)
    {
        Localizer = localizer;
    }
}