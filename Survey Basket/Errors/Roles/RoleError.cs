namespace Survey_Basket.Errors.Roles;

public static class RoleError
{
    public static readonly Error Notfound =
         new("Role.NotFound", "No Role was found with the given ID", StatusCodes.Status404NotFound);
    public static readonly Error DublicatedRole =
         new("Role.DublicatedRole", "Role is Dublicated ", StatusCodes.Status400BadRequest);

    public static readonly Error InvalidPermissions =
     new("Role.InvalidPermissions", "Invalid Permissions", StatusCodes.Status400BadRequest);

    //public static readonly Error DisabledUser =
    //    new("User.DisabledUser", "Disabled user, please contact your administrator", StatusCodes.Status401Unauthorized);

    //public static readonly Error InvalidJwtToken =
    //    new("User.InvalidJwtToken", "Invalid JWT token", StatusCodes.Status401Unauthorized);

    //public static readonly Error InvalidRefreshToken =
    //    new("User.InvalidRefreshToken", "Invalid refresh token", StatusCodes.Status401Unauthorized);

    //public static readonly Error DuplicatedEmail =
    //    new("User.DuplicatedEmail", "Another user with the same email already exists", StatusCodes.Status409Conflict);

    //public static readonly Error EmailNotConfirmed =
    //    new("User.EmailNotConfirmed", "Email is not confirmed", StatusCodes.Status401Unauthorized);

    //public static readonly Error InvalidCode =
    //    new("User.InvalidCode", "Invalid code", StatusCodes.Status401Unauthorized);

    //public static readonly Error DuplicatedConfirmation =
    //    new("User.DuplicatedConfirmation", "Email already confirmed", StatusCodes.Status400BadRequest);

}