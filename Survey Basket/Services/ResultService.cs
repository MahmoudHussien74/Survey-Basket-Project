using Survey_Basket.Abstractions;
using Survey_Basket.Contracts.Results;
using Survey_Basket.Errors.Polls;

namespace Survey_Basket.Services
{
    public class ResultService(ApplicationDbContext context):IResultService   
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<Result<PollVotesResponse>> getPollVotesAsync(int pollid, CancellationToken cancellationtoken = default)
        {
            var pollvotes = await _context.Polls
                                 .Where(x=>x.Id == pollid)
                                 .Select(x => new PollVotesResponse(
                                    x.Title,
                                    x.Votes.Select(v=>new VoteResponse(
                                      $"{v.User.FirstName} {v.User.LastName}",
                                          v.SubmittedOn,
                                          v.VoteAnswers.Select(a=>new QuestionAnswerResponse(
                                          a.Question.Content,
                                          a.Answer.Content
                                          ))
                                    ))
                                 )).SingleOrDefaultAsync(cancellationtoken);

            return pollvotes is null?
                 Result.Failure<PollVotesResponse>(PollErrors.Notfound)
                :Result.Success(pollvotes);
        }
        public async Task<Result<IEnumerable<VotesPerDayResponse>>> GetVotesPerDayAsync(int pollId, CancellationToken cancellationtoken = default)
        {
            var pollIsExsist = await _context.Polls.AnyAsync(x => x.Id == pollId, cancellationToken: cancellationtoken);

            if (!pollIsExsist)
                return Result.Failure<IEnumerable<VotesPerDayResponse>>(PollErrors.Notfound);

            var votesPerDay =await _context.Votes
                .Where(x=>x.PollId ==pollId)
                .GroupBy(x=>new {Date = DateOnly.FromDateTime(x.SubmittedOn)})
                .Select(g=>new VotesPerDayResponse(
                    g.Key.Date,
                    g.Count()
                )).ToListAsync(cancellationtoken);
                   
            return Result.Success<IEnumerable<VotesPerDayResponse>>(votesPerDay);
        }
        public async Task<Result<IEnumerable<VotesPerQuestionResponse>>> GetVotesPerQuestionAsync(int pollId, CancellationToken cancellationtoken = default)
        {
            var pollIsExsist = await _context.Polls.AnyAsync(x => x.Id == pollId, cancellationToken: cancellationtoken);

            if (!pollIsExsist)
                return Result.Failure<IEnumerable<VotesPerQuestionResponse>>(PollErrors.Notfound);

            var votesPerQuestion = await _context.VoteAnswers
                    .Where(x => x.Vote.PollId == pollId)
                    .Select(x => new VotesPerQuestionResponse(
                        x.Question.Content,
                        x.Question.Votes
                        .GroupBy(x => new { Answers = x.Answer.Id, AnswerContent = x.Answer.Content })
                        .Select(g => new VotesPerAnswerResponse(
                            g.Key.AnswerContent,
                            g.Count()
                        ))
                    ))
                    .ToListAsync(cancellationtoken);
                   
            return Result.Success<IEnumerable<VotesPerQuestionResponse>>(votesPerQuestion);
        }
    }
}
