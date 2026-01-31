using Survey_Basket.Abstractions;
using Survey_Basket.Contracts.Results;

namespace Survey_Basket.Services
{
    public interface IResultService
    {
        public Task<Result<PollVotesResponse>> getPollVotesAsync(int pollid, CancellationToken cancellationtoken = default);
        public  Task<Result<IEnumerable<VotesPerDayResponse>>> GetVotesPerDayAsync(int pollId, CancellationToken cancellationtoken = default);
        public Task<Result<IEnumerable<VotesPerQuestionResponse>>> GetVotesPerQuestionAsync(int pollId, CancellationToken cancellationtoken = default);

    }
}
