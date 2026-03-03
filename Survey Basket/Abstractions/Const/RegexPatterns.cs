namespace Survey_Basket.Abstractions.Const;

public static class RegexPatterns
{
    public const string Password =
        @"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[^a-zA-Z0-9]).{8,}$";
}
