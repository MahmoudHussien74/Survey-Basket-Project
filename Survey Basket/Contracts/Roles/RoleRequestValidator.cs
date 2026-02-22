namespace Survey_Basket.Contracts.Roles;
public class RoleRequestValidator:AbstractValidator<RoleRequest>
{
    public RoleRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty();

        RuleFor(x=>x.Permissions)
            .NotNull()
            .NotEmpty();

        RuleFor(x => x.Permissions)
            .Must(x => x.Distinct().Count() == x.Count)
            .WithMessage("You can not dublicated permission for the same role")
            .When(x => x.Permissions != null);

    }
}