namespace Survey_Basket.Contracts.Roles;

public record RoleRequest(
    string Name,
    IList<string>Permissions
);


