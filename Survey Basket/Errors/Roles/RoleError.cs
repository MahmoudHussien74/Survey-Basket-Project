namespace Survey_Basket.Errors.Roles;

public static class RoleError
{
    public static readonly Error Notfound =
         new("Role.NotFound", "No Role was found with the given ID", StatusCodes.Status404NotFound);
    public static readonly Error DublicatedRole =
         new("Role.DublicatedRole", "Role is Dublicated ", StatusCodes.Status400BadRequest);

    public static readonly Error InvalidPermissions =
     new("Role.InvalidPermissions", "Invalid Permissions", StatusCodes.Status400BadRequest);



}