namespace Survey_Basket.Contracts.Votes
{
    public record VoteAnswerRequest(
        int QuestionId,
        int AnswerId
    );
}
