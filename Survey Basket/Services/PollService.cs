using Survey_Basket.Abstractions;
using Survey_Basket.Errors.Polls;

namespace Survey_Basket.Services
{
    public class PollService(ApplicationDbContext context) : IPollService
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<IEnumerable<PollResponse>> GetAllAsync(CancellationToken cancellationToken) =>
            await _context.Polls
            .ProjectToType<PollResponse>()
            .AsNoTracking()
            .ToListAsync(cancellationToken);





        public async Task<Result<PollResponse>> GetAsync(int id, CancellationToken cancellationToken)
        {
            var poll = await _context.Polls.FindAsync(id, cancellationToken);

            if (poll is null)
                return Result.Failure<PollResponse>(PollErrors.Notfound);

            return Result.Success(poll.Adapt<PollResponse>());
        }

        public async Task<Result<PollResponse>> AddAsync(PollRequest poll, CancellationToken cancellationToken = default)
        {
            await _context.Polls.AddAsync(poll.Adapt<Poll>(), cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success(poll.Adapt<PollResponse>());
        }


        public async Task<Result> UpdateAsync(int id, PollRequest poll, CancellationToken cancellationToken)
        {
            var CurrentPoll = await _context.Polls.FindAsync(id, cancellationToken);


            if (CurrentPoll is null)
                return Result.Failure(PollErrors.Notfound);



            CurrentPoll!.Title = poll.Title;
            CurrentPoll.Summary = poll.Summary;
            CurrentPoll.StartsAt = poll.StartsAt;
            CurrentPoll.EndsAt = poll.EndsAt;


            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }

        public async Task<Result> Delete(int id, CancellationToken cancellationToken)
        {
            var poll = await _context.Polls.FindAsync(id, cancellationToken);

            if (poll is null)
                return Result.Failure(PollErrors.Notfound);

            _context.Polls.Remove(poll);
            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();

        }

        public async Task<Result> ToggleStatusAsync(int id, CancellationToken cancellationToken = default)
        {
            var poll = await _context.Polls.FindAsync(id, cancellationToken);


            if (poll is null)
                return Result.Failure(PollErrors.Notfound);

            poll.IsPublished = !poll.IsPublished;

            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }

    }
}
