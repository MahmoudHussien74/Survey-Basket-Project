namespace Survey_Basket.Contracts.Users;

public record UserProfileResponse(
    string Email,
    string FirstName,
    string LastName
);
