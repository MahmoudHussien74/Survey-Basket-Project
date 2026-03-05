using Survey_Basket.Abstractions.Const;

namespace Survey_Basket.Contracts.Users;

public class ChangePasswordRequestValidator:AbstractValidator<ChangePasswordRequest>
{
    public ChangePasswordRequestValidator()
    {
        RuleFor(x => x.CurrentPassword)
            .NotEmpty();

        RuleFor(x => x.NewPassword)
          .NotEmpty()
          .Matches(RegexPatterns.Password)
          .WithMessage("Password must be at least 8 characters long and contain at least one uppercase letter, one lowercase letter, one number, and one special character.");


    }
}
