using Survey_Basket.Abstractions.Const;

namespace Survey_Basket.Contracts.Authentication;

public class RegisterRequestValidator:AbstractValidator<RegisterRequest>
{
    public RegisterRequestValidator()
    {
        RuleFor(x=>x.FirstName)
            .MaximumLength(100)
            .NotEmpty();
        RuleFor(x=>x.LastName)
            .MaximumLength(100)
            .NotEmpty();

        RuleFor(x => x.Email)
            .EmailAddress()
            .NotEmpty();

        RuleFor(x => x.Password)
            .NotEmpty()
            .Matches(RegexPatterns.Password)
            .WithMessage("Password must be at least 8 characters long and contain at least one uppercase letter, one lowercase letter, one number, and one special character.");

    }
}
