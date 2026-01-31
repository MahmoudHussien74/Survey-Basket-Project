namespace Survey_Basket.Contracts.Results
{
    public record VotesPerDayResponse(
        DateOnly Date,
        int NumberOfVotes
    );
   
}
