using Survey_Basket.Abstractions;
using Survey_Basket.Contracts.Common;
using Survey_Basket.Contracts.Questions;

namespace Survey_Basket.Services
{
    public interface IQuestionService
    {
        Task<Result<QuestionResponse>>AddAsync(int PollId,QuestionRequest request,CancellationToken CancellationToken=default);
        Task<Result<PaginatedList<QuestionResponse>>>GetAllAsync(int PollId,RequestFilters filters, CancellationToken CancellationToken=default);
        Task<Result<IEnumerable<QuestionResponse>>> GetAvilableAsync(int PollId, string userId, CancellationToken CancellationToken = default);
        Task<Result<QuestionResponse>>GetAsync(int PollId,int id, CancellationToken CancellationToken=default);
        Task<Result> ToggleStatusAsync(int PollId,int id, CancellationToken CancellationToken=default);

        Task<Result> UpdateAsync(int pollId, int id, QuestionRequest request, CancellationToken cancellationToken);

    }
} 
