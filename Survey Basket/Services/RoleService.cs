using Survey_Basket.Abstractions.Const;
using Survey_Basket.Contracts.Roles;
using Survey_Basket.Errors.Roles;

namespace Survey_Basket.Services;

public class RoleService(RoleManager<ApplicationRole> roleManager,ApplicationDbContext context) : IRoleService
{
    private readonly RoleManager<ApplicationRole> _roleManager = roleManager;
    private readonly ApplicationDbContext _context = context;

    public async Task<IEnumerable<RoleResponse>> GetAllAsync(bool? includeDisabled = false ,CancellationToken cancellationToken = default!) =>
        await _roleManager.Roles
            .Where(x=>!x.IsDefult && (!x.IsDeleted|| (includeDisabled.HasValue || includeDisabled.Value)) )
            .ProjectToType<RoleResponse>()
            .ToListAsync(cancellationToken);       

    
    public async Task<Result<RoleDetailsResponse>> GetAsync(string id,CancellationToken cancellationToken)
    {
        if (await _roleManager.FindByIdAsync(id) is not { } role)
            return Result.Failure<RoleDetailsResponse>(RoleError.Notfound);

        

        var permissions = await _roleManager.GetClaimsAsync(role);

        var response = new RoleDetailsResponse(role.Id, role.Name!, role.IsDeleted, permissions.Select(x => x.Value));
     
        return Result.Success(response);
    }

    public async Task<Result<RoleDetailsResponse>> AddAsync(RoleRequest request, CancellationToken cancellationToken)
    {
        var nameIsExsist = await _roleManager.FindByNameAsync(request.Name);

        if (nameIsExsist is not null)
            return Result.Failure<RoleDetailsResponse>(RoleError.DublicatedRole);


        var allowedPermissions = Permissions.GetAllPermissions();

        if(request.Permissions.Except(allowedPermissions).Any())
            return Result.Failure<RoleDetailsResponse>(RoleError.InvalidPermissions);

        var role = new ApplicationRole
        {
            Name = request.Name,
            ConcurrencyStamp = Guid.NewGuid().ToString()
        };

        var result = await _roleManager.CreateAsync(role);

        if(result.Succeeded)
        {
            var permissions = request.Permissions
                .Select(x => new IdentityRoleClaim<string>
                {
                    ClaimType = Permissions.Type,
                    ClaimValue = x,
                    RoleId = role.Id
                });
            await _context.AddRangeAsync(permissions);
            await _context.SaveChangesAsync(cancellationToken);

            var response = new RoleDetailsResponse(role.Id, role.Name!, role.IsDeleted, request.Permissions);
        
            return Result.Success(response);
        
        }
        var error = result.Errors.First();


        return Result.Failure<RoleDetailsResponse>(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));
    }


}
