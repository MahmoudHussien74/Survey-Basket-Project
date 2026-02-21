using Survey_Basket.Abstractions.Const;
namespace Survey_Basket.Persistence.Configurations;
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


        builder.HasData(new User
        {
            Id = DefultUsers.AdminId,
            FirstName = "survey basket",
            LastName = "Admin",
            UserName = DefultUsers.AdminEmail,
            NormalizedUserName = DefultUsers.AdminEmail.ToUpper(),
            Email = DefultUsers.AdminEmail,
            NormalizedEmail = DefultUsers.AdminEmail.ToUpper(),
            SecurityStamp = DefultUsers.AdminSecurityStamp,
            ConcurrencyStamp = DefultUsers.AdminConcurrencyStamp,
            EmailConfirmed = true,
            PasswordHash = DefultUsers.AdminPassword
        });
    }
}
