namespace Survey_Basket.Contracts.Results
{
    public record VotesPerQuestionResponse(
        string Question,
        IEnumerable<VotesPerAnswerResponse> SelectedAnswers
    );
}
