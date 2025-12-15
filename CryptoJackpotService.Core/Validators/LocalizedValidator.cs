using CryptoJackpotService.Models.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace CryptoJackpotService.Core.Validators;

public abstract class LocalizedValidator<T>(IStringLocalizer<ISharedResource> localizer) : AbstractValidator<T>
{
    protected readonly IStringLocalizer<ISharedResource> Localizer = localizer;
}