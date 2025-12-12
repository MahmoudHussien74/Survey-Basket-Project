
using Survey_Basket.Entities;

namespace Survey_Basket.Persistence.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.OwnsMany(x => x.RefreshTokens)
                   .ToTable("RefreshTokens")
                   .WithOwner()
                   .HasForeignKey("UserId");

            builder.Property(x => x.FirstName)
                .HasMaxLength(100);
            builder.Property(x => x.LastName)
                .HasMaxLength(100);

        }
    }
}
