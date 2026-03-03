namespace Survey_Basket.Contracts.Authentication;

public class ConfiramEmailRequestValidator:AbstractValidator<ConfiramEmailRequest>
{
    public ConfiramEmailRequestValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty();

        RuleFor(x => x.Code)
            .NotEmpty();
    }
}
