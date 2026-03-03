namespace Survey_Basket.Contracts.Authentication;

public record ConfiramEmailRequest(
    string UserId,
    string Code
);

