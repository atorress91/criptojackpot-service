using CryptoJackpotService.Models.Constants;
using CryptoJackpotService.Models.Request.Lottery;
using CryptoJackpotService.Models.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;
namespace CryptoJackpotService.Core.Validators;

public class CreateLotteryRequestValidator : LocalizedValidator<CreateLotteryRequest>
{
    public CreateLotteryRequestValidator(IStringLocalizer<ISharedResource> localizer) : base(localizer)
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage(Localizer[ValidationMessages.Required])
            .MaximumLength(200).WithMessage(_ => Localizer[ValidationMessages.MaxLength, 200]);

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage(Localizer[ValidationMessages.Required])
            .MaximumLength(2000).WithMessage(_ => Localizer[ValidationMessages.MaxLength, 2000]);

        RuleFor(x => x.MinNumber)
            .GreaterThan(0).WithMessage(Localizer[ValidationMessages.InvalidMinNumber]);

        RuleFor(x => x.MaxNumber)
            .GreaterThan(0).WithMessage(Localizer[ValidationMessages.InvalidMaxNumber])
            .GreaterThan(x => x.MinNumber).WithMessage(Localizer[ValidationMessages.MaxNumberMustBeGreaterThanMin]);

        RuleFor(x => x.TotalSeries)
            .GreaterThan(0).WithMessage(Localizer[ValidationMessages.InvalidTotalSeries]);

        RuleFor(x => x.TicketPrice)
            .GreaterThan(0).WithMessage(Localizer[ValidationMessages.InvalidPrice]);

        RuleFor(x => x.MaxTickets)
            .GreaterThan(0).WithMessage(Localizer[ValidationMessages.InvalidMaxTickets]);

        RuleFor(x => x.StartDate)
            .NotEmpty().WithMessage(Localizer[ValidationMessages.Required])
            .GreaterThan(DateTime.UtcNow).WithMessage(Localizer[ValidationMessages.StartDateMustBeFuture]);

        RuleFor(x => x.EndDate)
            .NotEmpty().WithMessage(Localizer[ValidationMessages.Required])
            .GreaterThan(x => x.StartDate).WithMessage(Localizer[ValidationMessages.InvalidDateRange]);

        RuleFor(x => x.Status)
            .IsInEnum().WithMessage(Localizer[ValidationMessages.InvalidRequest]);
    }
}