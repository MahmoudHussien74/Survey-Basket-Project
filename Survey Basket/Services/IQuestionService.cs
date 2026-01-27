using Survey_Basket.Abstractions;
using Survey_Basket.Contracts.Questions;

namespace Survey_Basket.Services
{
    public interface IQuestionService
    {
        Task<Result<QuestionResponse>>AddAsync(int PollId,QuestionRequest request,CancellationToken CancellationToken=default);
        Task<Result<List<QuestionResponse>>>GetAllAsync(int PollId, CancellationToken CancellationToken=default);
        Task<Result<QuestionResponse>>GetAsync(int PollId,int id, CancellationToken CancellationToken=default);
        Task<Result> ToggleStatusAsync(int PollId,int id, CancellationToken CancellationToken=default);


    }
} 
