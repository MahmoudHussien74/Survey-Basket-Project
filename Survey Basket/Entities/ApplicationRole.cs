namespace Survey_Basket.Entities
{
    public class ApplicationRole:IdentityRole
    {
        public bool IsDefult { get; set; }
        public bool IsDeleted { get; set; }
    }
}
