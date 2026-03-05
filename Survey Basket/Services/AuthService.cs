using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.WebUtilities;
using Survey_Basket.Helpers;
using System.Data;
namespace Survey_Basket.Services;
public class AuthService(UserManager<User> userManager
    , IJwtProvider jwtProvider
    ,ApplicationDbContext context
    ,SignInManager<User> signInManager
    ,ILogger<AuthService> logger
    ,IHttpContextAccessor httpContextAccessor
    ,IEmailSender emailSender) : IAuthService
{
    private readonly UserManager<User> _userManager = userManager;
    private readonly IJwtProvider _jwtProvider = jwtProvider;
    private readonly ApplicationDbContext _context = context;
    private readonly SignInManager<User> _signInManager = signInManager;
    private readonly ILogger<AuthService> _logger = logger;
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
    private readonly IEmailSender _emailSender = emailSender;
    private readonly int _refreshTokenExpireation = 14;


    public async Task<Result<AuthResponse>> GetTokenAsync(string email, string password, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(email);

        if (user is null)
            return Result.Failure<AuthResponse>(UserError.InvalidCredentials);

        var result = await _signInManager.PasswordSignInAsync(user, password, false,false);

        if(result.Succeeded)
        {
            var (userRoles, roleClaims) = await GetUserPermissionsAsync(user);


            var (token, expireIn) = _jwtProvider.GenerateToken(user, userRoles, roleClaims);


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

        return Result.Failure<AuthResponse>(result.IsNotAllowed ? UserError.EmailNotConfirmed : UserError.InvalidCredentials);
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

        var (userRoles, roleClaims) = await GetUserPermissionsAsync(user);

        var (newToken, expireIn) = _jwtProvider.GenerateToken(user, userRoles, roleClaims);

        var newRefreshToken = GenerateRefreshToken();
        var newExpireationRefreshToken = DateTime.UtcNow.AddDays(_refreshTokenExpireation);
        user.RefreshTokens.Add(new RefreshToken
        {
            Token = newRefreshToken,
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

    public async Task<(IEnumerable<string> roles, IEnumerable<string> permissons)> GetUserPermissionsAsync(User user)
    {
        var userRoles = await _userManager.GetRolesAsync(user);


        //var roleClaims = await _context.Roles
        //    .Join(_context.RoleClaims,
        //      role => role.Id,
        //      claim => claim.RoleId,
        //      (role, claim) => new { role, claim }
        //    )
        //    .Where(x => userRoles.Contains(x.role.Name!))
        //    .Select(x=>x.claim.ClaimValue!)
        //    .Distinct()
        //    .ToListAsync();

        var permissions = await (from r in _context.Roles
                                 join p in _context.RoleClaims
                                 on r.Id equals (p.RoleId)
                                 where userRoles.Contains(r.Name!)
                                 select p.ClaimValue
                                 )
                                 .Distinct()
                                 .ToListAsync();
            
        return (userRoles, permissions);

    }

    public async Task<Result> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken)
    {
        var emailIsExsist = await _userManager.Users.AnyAsync(x => x.Email == request.Email, cancellationToken);

        if (emailIsExsist)
            return Result.Failure(UserError.DuplicatedEmail);

        var user = request.Adapt<User>();
        user.UserName = request.Email;

        var result = await _userManager.CreateAsync(user, request.Password);

        if(result.Succeeded)
        {
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

            _logger.LogInformation("Confirmation code : {code}", code);

            await SendConfirmationEmail(user, code);
           
            return Result.Success();
        }
        var error = result.Errors.First();

        return Result.Failure(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));
    }

    public async Task<Result> ConfirmEmailAsync(ConfiramEmailRequest request)
    {
        if (await _userManager.FindByIdAsync(request.UserId) is not { } user)
            return Result.Failure(UserError.InvalidCode);

        if(user.EmailConfirmed)
            return Result.Failure(UserError.DuplicatedConfirmation);

        var code = request.Code;
        try
        {
            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));

        }
        catch (FormatException)
        {
            return Result.Failure(UserError.InvalidCode);
        }

        var result = await _userManager.ConfirmEmailAsync(user, code);

        if (result.Succeeded)
            return Result.Success();
        
        var error = result.Errors.First();
        return Result.Failure(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));

    }
    public async Task<Result> ResendConfirmEmailAsync(ResendConfirmEmailRequest request)
    {
        if (await _userManager.FindByEmailAsync(request.Email) is not { } user)
            return Result.Success();

        if (user.EmailConfirmed)
            return Result.Failure(UserError.DuplicatedConfirmation);


        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

        _logger.LogInformation(message: "Confirmation code : {code}", code);

        await SendConfirmationEmail(user, code);

        return Result.Success();
    }
    public async Task<Result> SendResetPasswordCode(string email)
    {
        if (await _userManager.FindByEmailAsync(email) is not { } user)
            return Result.Success();

        if (!user.EmailConfirmed)
            return Result.Failure(UserError.EmailNotConfirmed);

        var code = await _userManager.GeneratePasswordResetTokenAsync(user);
        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

        _logger.LogInformation(message: "Reset code : {code}", code);

        await SendResetPasswordCodeAsync(user,code);

        return Result.Success();
    }
    public async Task<Result> ResetPasswordAsync(ResetPasswordRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);

        if (user is null || !user.EmailConfirmed )
            return Result.Failure(UserError.InvalidCode);

        IdentityResult result;

        try
        {
            var code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(request.Code));

            result = await _userManager.ResetPasswordAsync(user, code,request.NewPassword);


        }
        catch (FormatException)
        {
            result = IdentityResult.Failed(_userManager.ErrorDescriber.InvalidToken());
        }
        if(result.Succeeded)
            return Result.Success();

        var error = result.Errors.First();
        return Result.Failure(new Error(error.Code, error.Description, StatusCodes.Status401Unauthorized));
    }
    private async Task SendResetPasswordCodeAsync(User user,string code)
    {
        var origin = _httpContextAccessor.HttpContext?.Request.Headers.Origin;
        
        var emailBody = EmailBodyBuilder.GenerateEmailBody("ForgetPassword",
             new Dictionary<string, string>
            {
                    {"{{name}}",user.FirstName },
                    {"{{action_url}}",$"{origin}/auth/forgetPassword?email={user.Email}&code={code}"}
            });
    }
    private async Task SendConfirmationEmail(User user,string code)
    {
        var origin = _httpContextAccessor.HttpContext?.Request.Headers.Origin;

        var emailBody = EmailBodyBuilder.GenerateEmailBody("EmailConfirmation",
            new Dictionary<string, string>
            {
                    {"{{name}}",user.FirstName },
                    {"{{action_url}}",$"{origin}/auth/emailConfirmation?userId={user.Id}&code={code}"}
            });
        await _emailSender.SendEmailAsync(user.Email!, "✅ Survey Basket: Email Confirmation", emailBody);

    }
    public static string GenerateRefreshToken() => Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));

}


