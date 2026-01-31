namespace Survey_Basket.Contracts.Results
{
    public record PollVotesResponse(
        string Title,
        IEnumerable<VoteResponse> Votes
    );
    
    public record VoteResponse(
        string VoterName,
        DateTime VoteDate,
        IEnumerable<QuestionAnswerResponse> SelectedAnswers
    );
    public record QuestionAnswerResponse(
        string Question,
        string Answer
    );
    
}
