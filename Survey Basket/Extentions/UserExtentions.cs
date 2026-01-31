namespace Survey_Basket.Extentions
{
    public static class UserExtentions
    {
        public static string? GetUserId(this ClaimsPrincipal user) =>
            user.FindFirstValue(ClaimTypes.NameIdentifier);
    }
}
