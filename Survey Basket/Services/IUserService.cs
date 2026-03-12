using Survey_Basket.Contracts.Users;
namespace Survey_Basket.Services;
public interface IUserService
{
    Task<Result<UserProfileResponse>> GetProfileAsync(string userId);
    Task<Result> UpdateProfileAsync(string userId, UpdateProfileRequest request);
    Task<Result> ChangePasswordAsync(string userId, ChangePasswordRequest request);
    Task<IEnumerable<UserResponse>> GetAllAsync(CancellationToken cancellationToken);
    Task<Result<UserResponse>> GetAsync(string id);
    Task<Result<UserResponse>> AddAsync(CreateUserRequest request, CancellationToken cancellationToken);
    Task<Result> UpdateAsync(string id, UpdateUserRequest request, CancellationToken cancellationToken);
    Task<Result> ToggleStatus(string id);
    Task<Result> Unlock(string id);


}
