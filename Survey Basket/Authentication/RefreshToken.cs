using System.ComponentModel.DataAnnotations;

namespace Survey_Basket.Authentication
{
    [Owned]
    public class RefreshToken
    {
        public string Token { get; set; } = string.Empty;
        public DateTime ExpiresOn { get; set; }
        public DateTime CreationOn { get; set; } = DateTime.UtcNow;
        public DateTime? RevokedOn { get; set; } 

        public bool IsExpired => DateTime.UtcNow > ExpiresOn;
        public bool IsActive => RevokedOn is null && !IsExpired;

    }
}
