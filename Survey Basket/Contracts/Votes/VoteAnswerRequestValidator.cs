namespace Survey_Basket.Contracts.Votes
{
    public class VoteAnswerRequestValidator:AbstractValidator<VoteAnswerRequest>
    {
        public VoteAnswerRequestValidator()
        {
            RuleFor(x => x.AnswerId)
                .GreaterThan(0)
                .NotEmpty();
            
            RuleFor(x => x.QuestionId)
                .GreaterThan(0)
                .NotEmpty();
            


        }
    }
}
