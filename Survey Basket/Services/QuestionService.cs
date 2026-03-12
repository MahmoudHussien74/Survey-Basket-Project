using Microsoft.Extensions.Caching.Hybrid;
using System.Linq.Dynamic.Core;

namespace Survey_Basket.Services;

public class QuestionService(ApplicationDbContext context,HybridCache hybridCache) : IQuestionService
{
    private readonly ApplicationDbContext _context = context;
    private readonly HybridCache _hybridCache = hybridCache;
    private const string _cachePrefix = "avilableQuestions";
    public async Task<Result<QuestionResponse>> AddAsync(int PollId, QuestionRequest request, CancellationToken CancellationToken = default)
    {
        var pollIsExsist = await _context.Polls.AnyAsync(x => x.Id == PollId, cancellationToken: CancellationToken);

        if (!pollIsExsist)
            return Result.Failure<QuestionResponse>(PollErrors.Notfound);

        var questionIsExsit = await _context.Questions.AnyAsync(x => x.Content == request.Content && x.PollId == PollId, cancellationToken: CancellationToken);

        if (questionIsExsit)
            return Result.Failure<QuestionResponse>(QuestionErrors.DuplicateQuestionContent);

        var question = request.Adapt<Question>();

        question.PollId = PollId;

        request.Answers.ForEach(answer => question.Answers.Add(new Answer { Content = answer }));
        await _context.AddAsync(question, CancellationToken);
        await _context.SaveChangesAsync(CancellationToken);

        await _hybridCache.RemoveAsync($"{_cachePrefix}-{PollId}", CancellationToken);

        return Result.Success(question.Adapt<QuestionResponse>());
    }

    public async Task<Result<PaginatedList<QuestionResponse>>> GetAllAsync(int PollId, RequestFilters filters, CancellationToken CancellationToken = default)
    {

        var pollIsExsist = await _context.Polls.AnyAsync(x => x.Id == PollId, cancellationToken: CancellationToken);
        if (!pollIsExsist)
            return Result.Failure<PaginatedList<QuestionResponse>>(PollErrors.Notfound);

        var query = _context.Questions
                            .Where(x => x.PollId == PollId);
        //&& (string.IsNullOrEmpty(filters.SearchValue)||x.Content.Contains(filters.SearchValue)))

        if (!string.IsNullOrEmpty(filters.SearchValue))
        {
            query = query.Where(x => x.Content.Contains(filters.SearchValue));
        }
        if (string.IsNullOrEmpty(filters.SortColumn))
        {
            query = query.OrderBy($"{filters.SortColumn} {filters.SortDirection}");
        }

        var source = query.Include(x => x.Answers)
                          .ProjectToType<QuestionResponse>()
                          .AsNoTracking();

        var questions = await PaginatedList<QuestionResponse>.CreateAsync(source,filters.PageSize, filters.PageSize,CancellationToken);
        return Result.Success(questions);

    }

    public async Task<Result<IEnumerable<QuestionResponse>>> GetAvilableAsync(int PollId, string userId, CancellationToken CancellationToken = default)
    {
        var hasVote = await _context.Votes.AnyAsync(x => x.PollId == PollId && x.UserId == userId, cancellationToken: CancellationToken);

        if (hasVote)
            return Result.Failure<IEnumerable<QuestionResponse>>(VoteErrors.DuplicateVote);

        var pollIsExsist = await _context.Polls
            .AnyAsync(x => x.Id == PollId
            && x.IsPublished && x.StartsAt <= DateOnly.FromDateTime(DateTime.UtcNow) 
            && x.EndsAt >= DateOnly.FromDateTime(DateTime.UtcNow)
            , cancellationToken: CancellationToken
            );

        if (!pollIsExsist)
            return Result.Failure<IEnumerable<QuestionResponse>>(PollErrors.Notfound);

        var cacheKey = $"{_cachePrefix}-{PollId}";


        var questions = await _hybridCache.GetOrCreateAsync<IEnumerable<QuestionResponse>>
            (cacheKey,
            async cacheEntry =>
                                await _context.Questions
                                 .Where(x => x.PollId == PollId && x.IsActive)
                                 .Include(x => x.Answers)
                                 .Select(x => new QuestionResponse(
                                     x.Id,
                                     x.Content,
                                     x.Answers
                                     .Where(x => x.IsActive).Select(answer => new AnswerResponse(answer.Id, answer.Content))
                                 ))
                                 .AsNoTracking()
                                 .ToListAsync(CancellationToken), cancellationToken: CancellationToken
            );



        return Result.Success(questions);
    }
    public async Task<Result<QuestionResponse>> GetAsync(int PollId, int id, CancellationToken CancellationToken = default)
    {
        var pollIsExsist = _context.Polls.AnyAsync(x => x.Id == PollId, cancellationToken: CancellationToken);

        if (!pollIsExsist.Result)
            return Result.Failure<QuestionResponse>(PollErrors.Notfound);

        var question = await _context.Questions
                                   .Where(x => x.PollId == PollId && x.Id == id)
                                   .Include(x => x.Answers)
                                   .ProjectToType<QuestionResponse>()
                                   .SingleOrDefaultAsync(CancellationToken);

        if (question is null)
            return Result.Failure<QuestionResponse>(QuestionErrors.QuestionNotfound);

        return Result.Success(question);

    }

    public async Task<Result> ToggleStatusAsync(int PollId, int id, CancellationToken CancellationToken = default)
    {
        var pollIsExsist = await _context.Polls.AnyAsync(x => x.Id == PollId, cancellationToken: CancellationToken);
        if (!pollIsExsist)
            return Result.Failure<QuestionResponse>(PollErrors.Notfound);
        var question = await _context.Questions
                                  .Where(x => x.PollId == PollId && x.Id == id)
                                  .SingleOrDefaultAsync(CancellationToken);

        if (question is null)
            return Result.Failure<QuestionResponse>(QuestionErrors.QuestionNotfound);

        question.IsActive = !question.IsActive;
        await _context.SaveChangesAsync(CancellationToken);
        await _hybridCache.RemoveAsync($"{_cachePrefix}-{PollId}", CancellationToken);

        return Result.Success();
    }

    public async Task<Result> UpdateAsync(int pollId, int id, QuestionRequest request, CancellationToken cancellationToken)
    {

        var questionIsExsist = await _context.Questions
                                            .AnyAsync(x => x.PollId == pollId
                                                              && x.Id != id
                                                              && x.Content == request.Content,
                                                              cancellationToken);


        if (questionIsExsist)
            return Result.Failure(QuestionErrors.DuplicateQuestionContent);

        var question = await _context.Questions
                                     .Include(x => x.Answers)
                                     .SingleOrDefaultAsync(x => x.PollId == pollId && x.Id == id, cancellationToken: cancellationToken);

        if (question is null)
            return Result.Failure(QuestionErrors.QuestionNotfound);

        question.Content = request.Content;

        var cuurentAnswers = question.Answers.Select(x => x.Content).ToList();

        var newAnswers = request.Answers.Except(cuurentAnswers).ToList();

        newAnswers.ForEach(answer =>
        {
            question.Answers.Add(new Answer
            {
                Content = answer
            });
        });

        question.Answers.ToList().ForEach(answer =>
        {
            answer.IsActive = request.Answers.Contains(answer.Content);
        });

        await _context.SaveChangesAsync(cancellationToken);
        await _hybridCache.RemoveAsync($"{_cachePrefix}-{pollId}", cancellationToken);

        return Result.Success();
    }
}
