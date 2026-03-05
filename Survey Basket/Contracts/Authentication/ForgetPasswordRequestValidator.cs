namespace Survey_Basket.Contracts.Authentication;

public class ForgetPasswordRequestValidator:AbstractValidator<ForgetPasswordRequest>
{
    public ForgetPasswordRequestValidator()
    {
        RuleFor(x => x.Email)
              .EmailAddress()
              .NotEmpty();
    }
}
