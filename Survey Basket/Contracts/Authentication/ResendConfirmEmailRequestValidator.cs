namespace Survey_Basket.Contracts.Authentication;

public class ResendConfirmEmailRequestValidator:AbstractValidator<ResendConfirmEmailRequest>
{
    public ResendConfirmEmailRequestValidator()
    {
        RuleFor(x => x.Email)
                .EmailAddress()
                .NotEmpty();
    }
}
