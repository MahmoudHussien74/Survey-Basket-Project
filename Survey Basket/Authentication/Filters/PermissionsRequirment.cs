using Microsoft.Extensions.Options;
using Survey_Basket.Abstractions.Const;

namespace Survey_Basket.Authentication.Filters;

public class PermissionsRequirment(string permission):IAuthorizationRequirement
{
    public string Permission { get; } = permission;
}
public class HasPermissionAttribute(string permission) : AuthorizeAttribute(permission)
{

}

public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionsRequirment>
{
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionsRequirment requirement)
    {
        //var user = context.User.Identity;
        //if (!user.IsAuthenticated)
        //    return;


        if (context.User.Identity is not { IsAuthenticated: true }
        || !context.User.Claims.Any(x => x.Value == requirement.Permission && x.Type == Permissions.Type))
            return;

        context.Succeed(requirement);
        return ;
    }
}

public class PermissionAuthorizationPolicyProvider(IOptions<AuthorizationOptions> options) 
    : DefaultAuthorizationPolicyProvider(options)
{
    private readonly AuthorizationOptions _authorizationOptions = options.Value;
    public override async Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
    {

        var policy = await base.GetPolicyAsync(policyName);

        if (policy is not null)
            return policy;
        var permissionPolicy = new AuthorizationPolicyBuilder()
            .AddRequirements(new PermissionsRequirment(policyName))
            .Build();

        _authorizationOptions.AddPolicy(policyName, permissionPolicy);
        return permissionPolicy;
    }
}