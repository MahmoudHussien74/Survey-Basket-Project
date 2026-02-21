namespace Survey_Basket.Contracts.Roles;

public record RoleDetailsResponse(
    string Id,
    string Name,
    bool IsDeleted,
    IEnumerable<string> Permissions
);
