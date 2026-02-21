using Survey_Basket.Abstractions.Const;
namespace Survey_Basket.Persistence.Configurations;

public class UserRoleConfiguration : IEntityTypeConfiguration<IdentityUserRole<string>>
{
    public void Configure(EntityTypeBuilder<IdentityUserRole<string>> builder)
    {
      
        builder.HasData(new IdentityUserRole<string>
        {
            UserId = DefultUsers.AdminId,
            RoleId = DefultRoles.AdminRoleId
        });
    }
}