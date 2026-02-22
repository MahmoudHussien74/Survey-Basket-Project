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

        return result.IsFailure ? result.ToProblem() : CreatedAtAction(nameof(Get),new {result.Value.Id},result.Value);

    }
    [HttpPut("{id}")]
    [HasPermission(Permissions.UpdateRoles)]
    public async Task<IActionResult> UpdateAsync([FromRoute]string id, [FromBody]RoleRequest request,CancellationToken cancellationToken)
    {
        var result = await _roleService.UpdateAsync(id,request, cancellationToken);

        return result.IsFailure ? result.ToProblem() : NoContent();

    }
    [HttpPut("{id}/toggle-status")]
    [HasPermission(Permissions.UpdateRoles)]
    public async Task<IActionResult> ToggleStatusAsync([FromRoute]string id,CancellationToken cancellationToken)
    {
        var result = await _roleService.ToggleAsync(id, cancellationToken);

        return result.IsFailure ? result.ToProblem() : NoContent();
    }
}

