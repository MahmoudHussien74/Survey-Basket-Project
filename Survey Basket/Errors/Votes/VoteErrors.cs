using Survey_Basket.Abstractions;

namespace Survey_Basket.Errors.Votes
{
    public static class VoteErrors
    {
        public static readonly Error Notfound =
           new("Vote.NotFound", "No Vote was found with the given ID", StatusCodes.Status404NotFound);

        public static readonly Error InvalidQuestiontfound =
           new("Vote.InvalidQuestion", "Invalid Question", StatusCodes.Status404NotFound);

        public static readonly Error DuplicateVote =
            new("Vote.DuplicateVote", "this user already voted before for this poll", StatusCodes.Status409Conflict);
    }
}
