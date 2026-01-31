using Survey_Basket.Abstractions;
using Survey_Basket.Contracts.Votes;
using Survey_Basket.Errors.Polls;
using Survey_Basket.Errors.Votes;

namespace Survey_Basket.Services
{
    public class VoteService(ApplicationDbContext context) : IVoteService
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<Result> AddAsync(int pollId, string userId, VoteRequest request, CancellationToken cancellationToken = default)
        {
            var hasVote = await _context.Votes.AnyAsync(x => x.PollId == pollId && x.UserId == userId );

            if (hasVote)
                return Result.Failure(VoteErrors.DuplicateVote);

            var pollIsExsist = await _context.Polls.AnyAsync(x => x.Id == pollId && x.IsPublished && x.StartsAt <= DateOnly.FromDateTime(DateTime.UtcNow) && x.EndsAt >= DateOnly.FromDateTime(DateTime.UtcNow), cancellationToken);

            if (!pollIsExsist)
                return Result.Failure(PollErrors.Notfound);

            var availableQuestions = await _context.Questions
                .Where(x => x.PollId == pollId && x.IsActive)
                .Select(x => x.Id)
                .ToListAsync(cancellationToken);
            if (!request.Answers.Select(x => x.QuestionId).SequenceEqual(availableQuestions))
                return Result.Failure(VoteErrors.InvalidQuestiontfound);

            var vote = new Vote
            {
                PollId = pollId,
                UserId = userId,
                VoteAnswers = request.Answers.Adapt<IEnumerable<VoteAnswer>>().ToList()

            };



            await _context.AddAsync(vote,cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }

    }
}
