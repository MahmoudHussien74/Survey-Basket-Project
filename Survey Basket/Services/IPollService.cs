using Survey_Basket.Abstractions;

namespace Survey_Basket.Services
{
    public interface IPollService
    {
        Task<IEnumerable<PollResponse>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<IEnumerable<PollResponse>> GetCurrentAsync(CancellationToken cancellationToken);
        Task<Result<PollResponse>> GetAsync(int id, CancellationToken cancellationToken = default);
        Task<Result<PollResponse>> AddAsync(PollRequest poll, CancellationToken cancellationToken = default);
        Task<Result> UpdateAsync(int id, PollRequest request, CancellationToken cancellationToken = default);
        Task<Result> Delete(int id, CancellationToken cancellationToken = default);
        Task<Result> ToggleStatusAsync(int id, CancellationToken cancellationToken = default);
    }
}
