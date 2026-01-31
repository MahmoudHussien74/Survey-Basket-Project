namespace Survey_Basket.Contracts.Votes
{
    public record VoteRequest(
      IEnumerable<VoteAnswerRequest> Answers  
    );   
}
