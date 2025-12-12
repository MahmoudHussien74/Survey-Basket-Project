using Survey_Basket.Abstractions;

namespace Survey_Basket.Errors.Polls
{
    public class PollErrors
    {
        public static readonly Error Notfound =
            new("Poll.NotFound", "No Poll was found with the given ID", StatusCodes.Status404NotFound);

        public static readonly Error DuplicatePollTitle =
            new("Poll.DuplicatePollTitle", "Another Poll With the same title is already exists", StatusCodes.Status400BadRequest);
    }
}
