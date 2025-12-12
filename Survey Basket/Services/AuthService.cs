using Survey_Basket.Abstractions;
using Survey_Basket.Errors.User;
using System.Security.Cryptography;
namespace Survey_Basket.Services
{
    public class AuthService(UserManager<User> userManager, IJwtProvider jwtProvider) : IAuthService
    {
        private readonly UserManager<User> _userManager = userManager;
        private readonly IJwtProvider _jwtProvider = jwtProvider;
        private readonly int _refreshTokenExpireation = 14;

        public async Task<Result<AuthResponse>> GetTokenAsync(string email, string password, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user is null)
                return Result.Failure<AuthResponse>(UserError.InvalidCredentials);

            var isValidPassword = await _userManager.CheckPasswordAsync(user, password);

            if (!isValidPassword)
                return Result.Failure<AuthResponse>(UserError.InvalidCredentials);

            var (token, expireIn) = _jwtProvider.GenerateToken(user);


            var refreshToken = GenerateRefreshToken();
            var expireationRefreshToken = DateTime.UtcNow.AddDays(_refreshTokenExpireation);

            user.RefreshTokens.Add(new RefreshToken
            {
                Token = refreshToken,
                ExpiresOn = expireationRefreshToken
            });


            await _userManager.UpdateAsync(user);


            var response = new AuthResponse(user.Id, user.Email!, user.FirstName, user.LastName, token, expireIn, refreshToken, expireationRefreshToken);

            return Result.Success(response);
        }

        public async Task<Result<AuthResponse>> GetRefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken = default)
        {

            var userId = _jwtProvider.ValidateToken(token);

            if (userId is null)
                return Result.Failure<AuthResponse>(UserError.InvalidCredentials);

            var user = await _userManager.FindByIdAsync(userId);

            if (user is null)
                return Result.Failure<AuthResponse>(UserError.InvalidCredentials);


            var userRefreshToken = user.RefreshTokens.SingleOrDefault(x => x.Token == refreshToken && x.IsActive);

            if (userRefreshToken is null)
                return Result.Failure<AuthResponse>(UserError.InvalidCredentials);


            userRefreshToken.RevokedOn = DateTime.UtcNow;

            var (newToken, expireIn) = _jwtProvider.GenerateToken(user);
            var newRefreshToken = GenerateRefreshToken();
            var newExpireationRefreshToken = DateTime.UtcNow.AddDays(_refreshTokenExpireation);
            user.RefreshTokens.Add(new RefreshToken
            {
                Token = newToken,
                ExpiresOn = newExpireationRefreshToken
            });

            await _userManager.UpdateAsync(user);
            var response = new AuthResponse(user.Id, user.Email!, user.FirstName, user.LastName, token, expireIn, newRefreshToken, newExpireationRefreshToken);

            return Result.Success(response);
        }
        public async Task<Result<bool>> RevokeRefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken = default)
        {
            var userId = _jwtProvider.ValidateToken(token);

            if (userId is null)
                return Result.Failure<bool>(UserError.InvalidCredentials);


            var user = await _userManager.FindByIdAsync(userId);

            if (user is null)
                return Result.Failure<bool>(UserError.InvalidCredentials);

            var userRefreshToken = user.RefreshTokens.SingleOrDefault(x => x.Token == refreshToken && x.IsActive);

            if (userRefreshToken is null)
                return Result.Failure<bool>(UserError.InvalidCredentials);

            userRefreshToken.RevokedOn = DateTime.UtcNow;


            await _userManager.UpdateAsync(user);



            return Result.Success(true);
        }

        public static string GenerateRefreshToken() => Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
    }
}
