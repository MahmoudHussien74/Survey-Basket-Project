using Survey_Basket.Abstractions.Const;

namespace Survey_Basket.Contracts.Users;

public class CreateUserRequestValidator:AbstractValidator<CreateUserRequest>
{
    public CreateUserRequestValidator()
    {
        RuleFor(x => x.Email)
            .EmailAddress()
            .NotEmpty();

        RuleFor(x => x.FirstName)
            .NotEmpty()
            .Length(3, 100);
        RuleFor(x => x.LastName)
            .NotEmpty()
            .Length(3, 100);

        RuleFor(x => x.Password)
        .NotEmpty()
        .Matches(RegexPatterns.Password)
        .WithMessage("Password must be at least 8 characters long and contain at least one uppercase letter, one lowercase letter, one number, and one special character.");


        RuleFor(x => x.Roles)
            .Must(x => x.Distinct().Count() == x.Count)
            .WithMessage("You cannot add duplicated role for the same user")
            .When(x => x.Roles != null);
            
    }
}
