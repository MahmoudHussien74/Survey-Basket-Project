using Survey_Basket.Contracts.Roles;
namespace Survey_Basket.Services;
public interface IRoleService
{
    public  Task<IEnumerable<RoleResponse>> GetAllAsync(bool? includeDisabled = false, CancellationToken cancellationToken = default!);
    public  Task<Result<RoleDetailsResponse>> GetAsync(string id,CancellationToken cancellationToken);
    public  Task<Result<RoleDetailsResponse>> AddAsync(RoleRequest request, CancellationToken cancellationToken);
    public  Task<Result> UpdateAsync(string id, RoleRequest request, CancellationToken cancellationToken);
    public  Task<Result> ToggleAsync(string id, CancellationToken cancellationToken);

}