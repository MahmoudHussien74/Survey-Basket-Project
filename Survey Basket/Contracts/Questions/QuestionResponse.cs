using Survey_Basket.Contracts.Answers;

namespace Survey_Basket.Contracts.Questions
{
    public record QuestionResponse(
        int Id,
        string Content,
        IEnumerable<AnswerResponse> Answers
    ); 
}
