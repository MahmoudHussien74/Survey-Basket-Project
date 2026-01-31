namespace Survey_Basket.Contracts.Results
{
    public record VotesPerAnswerResponse(
            string Answer,
            int Count
    );
}
