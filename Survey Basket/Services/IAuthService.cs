using Survey_Basket.Abstractions;

namespace Survey_Basket.Services
{
    public interface IAuthService
    {
        public Task<Result<AuthResponse>> GetTokenAsync(string email, string password,CancellationToken cancellationToken);
        Task<Result<AuthResponse>> GetRefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken = default!);
        Task<Result<bool>> RevokeRefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken = default);


    }
}
