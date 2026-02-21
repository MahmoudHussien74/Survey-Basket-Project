using Survey_Basket.Abstractions;

namespace Survey_Basket.Errors.User;

public static class UserError
{
    public static readonly Error InvalidCredentials =
     new("User.InvalidCredentials", "Invalid email/password", StatusCodes.Status401Unauthorized);

    public static readonly Error DisabledUser =
        new("User.DisabledUser", "Disabled user, please contact your administrator", StatusCodes.Status401Unauthorized);

    public static readonly Error InvalidJwtToken =
        new("User.InvalidJwtToken", "Invalid JWT token", StatusCodes.Status401Unauthorized);

    public static readonly Error InvalidRefreshToken =
        new("User.InvalidRefreshToken", "Invalid refresh token", StatusCodes.Status401Unauthorized);

    public static readonly Error DuplicatedEmail =
        new("User.DuplicatedEmail", "Another user with the same email already exists", StatusCodes.Status409Conflict);

    public static readonly Error EmailNotConfirmed =
        new("User.EmailNotConfirmed", "Email is not confirmed", StatusCodes.Status401Unauthorized);

    public static readonly Error InvalidCode =
        new("User.InvalidCode", "Invalid code", StatusCodes.Status401Unauthorized);

    public static readonly Error DuplicatedConfirmation =
        new("User.DuplicatedConfirmation", "Email already confirmed", StatusCodes.Status400BadRequest);




}
