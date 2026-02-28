using Microsoft.AspNetCore.RateLimiting;

namespace Survey_Basket.Controllers;

[Route("[controller]")]
[ApiController]
[EnableRateLimiting("ipLimiter")]
public class AuthController(IAuthService authService,ILogger<AuthController> logger) : ControllerBase
{
    private readonly IAuthService _authService = authService;
    private readonly ILogger<AuthController> _logger = logger;

    [HttpPost("")]
    public async Task<IActionResult> LoginAsync(LoginRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("logging with email: {Email} and Password: {Password}", request.Email,request.Password);
        var authResult = await _authService.GetTokenAsync(request.Email, request.Password, cancellationToken);

        return authResult.IsFailure ? BadRequest("Invalid Email / Password")
            : Ok(authResult.Value);

    }
    [HttpPost("refresh")]
    public async Task<IActionResult> RefreshAsync(RefreshTokenRequest request, CancellationToken cancellationToken)
    {
        var authResult = await _authService.GetRefreshTokenAsync(request.Token, request.RefreshToken, cancellationToken);

        return authResult.IsFailure ? BadRequest("Invalid token")
            : Ok(authResult.Value);

    }

    [HttpPost("revoke-refresh-token")]
    public async Task<IActionResult> RevokeRefreshTokenAsync(RefreshTokenRequest request, CancellationToken cancellationToken)
    {
        var IsRevoked = await _authService.RevokeRefreshTokenAsync(request.Token, request.RefreshToken, cancellationToken);

        return IsRevoked.IsSuccess ? Ok() : BadRequest("Invalid token");

    }
}
