using CryptoJackpotService.Models.Request;
using FluentValidation;

namespace CryptoJackpotService.Core.Validators;

public class CreateUserRequestValidator : AbstractValidator<CreateUserRequest>
{
    public CreateUserRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty();

        RuleFor(x => x.LastName)
            .NotEmpty();

        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(8)
            .MaximumLength(16)
            .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter")
            .Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter")
            .Matches("[0-9]").WithMessage("Password must contain at least one number")
            .Matches("[^a-zA-Z0-9]").WithMessage("Password must contain at least one special character");

        RuleFor(x => x.RoleId)
            .NotEmpty()
            .GreaterThan(0);

        RuleFor(x => x.CountryId)
            .NotEmpty()
            .GreaterThan(0);

        RuleFor(x => x.StatePlace)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.City)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.Address)
            .MaximumLength(200)
            .When(x => !string.IsNullOrEmpty(x.Address));

        RuleFor(x => x.ImagePath)
            .MaximumLength(500)
            .When(x => !string.IsNullOrEmpty(x.ImagePath));

        RuleFor(x => x.GoogleAccessToken)
            .MaximumLength(2000)
            .When(x => !string.IsNullOrEmpty(x.GoogleAccessToken));

        RuleFor(x => x.GoogleRefreshToken)
            .MaximumLength(2000)
            .When(x => !string.IsNullOrEmpty(x.GoogleRefreshToken));
    }
}