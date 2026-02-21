using Survey_Basket.Abstractions.Const;
namespace Survey_Basket.Persistence.Configurations;
public class RoleConfiguration : IEntityTypeConfiguration<ApplicationRole>
{
    public void Configure(EntityTypeBuilder<ApplicationRole> builder)
    {
      
        var passwordHasher = new PasswordHasher<User>();

        builder.HasData([
            new ApplicationRole
            {
               Id =DefultRoles.AdminRoleId,
               Name = DefultRoles.Admin,
               NormalizedName = DefultRoles.Admin.ToUpper(),
               ConcurrencyStamp = DefultRoles.AdminRoleConcurrencyStamp,
            },
            new ApplicationRole
            {
                Id = DefultRoles.MemberRoleId,
                Name = DefultRoles.Member,
                NormalizedName = DefultRoles.Member.ToUpper(),
                ConcurrencyStamp = DefultRoles.MemberRoleConcurrencyStamp,
                IsDefult = true
            }
        ]);
    }
}
