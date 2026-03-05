namespace Survey_Basket.Contracts.Users;

public record ChangePasswordRequest(
    string CurrentPassword,    
    string NewPassword    
);

