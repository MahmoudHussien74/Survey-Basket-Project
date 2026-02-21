using Survey_Basket.Abstractions.Const;
using Survey_Basket.Authentication.Filters;
using Survey_Basket.Contracts.Roles;

namespace Survey_Basket.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RolesController(IRoleService roleService) : ControllerBase
{
    private readonly IRoleService _roleService = roleService;

    [HttpGet("")]
    [HasPermission(Permissions.GetRoles)]
    public async Task<IActionResult> GetAll([FromQuery] bool includeDisabled,CancellationToken cancellationToken)
    {
        var roles = await _roleService.GetAllAsync(includeDisabled, cancellationToken);

        return Ok(roles);
    }
    [HttpGet("{id}")]
    [HasPermission(Permissions.GetRoles)]

    public async Task<IActionResult> Get([FromRoute] string id,CancellationToken cancellationToken)
    {
        var result = await _roleService.GetAsync(id, cancellationToken);

        return result.IsFailure?
            result.ToProblem()
            :Ok(result.Value);
    }

    [HttpPost("")]
    [HasPermission(Permissions.AddRoles)]
    public async Task<IActionResult> AddAsync([FromBody]RoleRequest request,CancellationToken cancellationToken)
    {
        var result = await _roleService.AddAsync(request, cancellationToken);

        return result.IsFailure ? result.ToProblem() : Ok(result.Value);

    }

}

