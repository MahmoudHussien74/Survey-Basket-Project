namespace Survey_Basket.Authentication
{
    public interface IJwtProvider
    {
        (string token, int expiresIn) GenerateToken(User user,IEnumerable<string>roles,IEnumerable<string>permissions);
         public string? ValidateToken(string token);

    }
}
