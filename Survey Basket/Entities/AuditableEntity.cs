namespace Survey_Basket.Entities
{
    public class AuditableEntity
    {
        public string CreatedById { get; set; } = string.Empty;
        public DateTime CreationOn { get; set; } = DateTime.UtcNow;
        public string? UpdatedById { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public User CreatedBy { get; set; } = default!;
        public User? UpdatedBy { get; set; }
    }
}
