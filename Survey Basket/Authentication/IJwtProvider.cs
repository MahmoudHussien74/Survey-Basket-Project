namespace Survey_Basket.Authentication
{
    public interface IJwtProvider
    {
        (string token, int expiresIn) GenerateToken(User user);
         public string? ValidateToken(string token);

    }
}
