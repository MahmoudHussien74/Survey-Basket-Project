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

    public async Task<Result> UpdateAsync(string id,RoleRequest request, CancellationToken cancellationToken)
    {
        var roleNameIsExsist = await _context.Roles.AnyAsync(x=>x.Name == request.Name && x.Id !=id, cancellationToken);

        if(roleNameIsExsist)
            return Result.Failure<RoleDetailsResponse>(RoleError.DublicatedRole);

        var role = await _roleManager.FindByIdAsync(id);
        if(role is  null)
            return Result.Failure<RoleDetailsResponse>(RoleError.Notfound);

        role.Name = request.Name;

        var result = await _roleManager.UpdateAsync(role);
        if (result.Succeeded)
        {
            var currentPermissions = await _context.RoleClaims
                .Where(x => x.RoleId == role.Id && x.ClaimType == Permissions.Type)
                .Select(x=>x.ClaimValue)
                .ToListAsync(cancellationToken);


            var newPermissions = request.Permissions.Except(currentPermissions).Select(x => new IdentityRoleClaim<string>
            {
                ClaimType = Permissions.Type,
                ClaimValue = x,
                RoleId = role.Id
            });

            var removerdPermissions = currentPermissions.Except(request.Permissions);

            await _context.RoleClaims
                .Where(x => x.RoleId == role.Id && removerdPermissions.Contains(x.ClaimValue) &&
                            x.ClaimType == Permissions.Type)
                .ExecuteDeleteAsync(cancellationToken);
            
            await _context.AddRangeAsync(newPermissions, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
        var error = result.Errors.First();
        return Result.Failure(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));

    
    }
    public async Task<Result> ToggleAsync(string id, CancellationToken cancellationToken)
    {
        var role = await _roleManager.FindByIdAsync(id);
        if(role is  null)
            return Result.Failure<RoleDetailsResponse>(RoleError.Notfound);

        role.IsDeleted = !role.IsDeleted;

        var result = await _roleManager.UpdateAsync(role);
        if (result.Succeeded)
            return Result.Success();

        var error = result.Errors.First();
        return Result.Failure(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));

    }

}
