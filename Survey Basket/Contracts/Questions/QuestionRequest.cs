namespace Survey_Basket.Contracts.Questions
{
    public record QuestionRequest(
        string Content,
        List<string> Answers
    );
    
    
}
