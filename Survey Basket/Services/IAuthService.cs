using Survey_Basket.Abstractions;

namespace Survey_Basket.Services;

public interface IAuthService
{
    Task<Result<AuthResponse>> GetTokenAsync(string email, string password,CancellationToken cancellationToken);
    Task<Result<AuthResponse>> GetRefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken = default!);
    Task<Result<bool>> RevokeRefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken = default);
    Task<Result> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken);
    Task<Result> ConfirmEmailAsync(ConfiramEmailRequest request);
    Task<Result> ResendConfirmEmailAsync(ResendConfirmEmailRequest request);
    Task<Result> SendResetPasswordCode(string email);
    Task<Result> ResetPasswordAsync(ResetPasswordRequest request);


}
