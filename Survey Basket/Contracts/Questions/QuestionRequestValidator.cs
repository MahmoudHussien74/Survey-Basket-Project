namespace Survey_Basket.Contracts.Questions
{
    public class QuestionRequestValidator:AbstractValidator<QuestionRequest>
    {
        public QuestionRequestValidator()
        {
            RuleFor(x => x.Content)
                .NotEmpty()
                .WithMessage("Content is required.")
                .Length(3,1000)
                .WithMessage("Content cannot exceed 500 characters.");


            RuleFor(x => x.Answers)
                .NotNull();

            RuleFor(x => x.Answers)
                .Must(c => c.Count > 1)
                .WithMessage("You must add at least two answers.")
                .When(x => x.Answers != null);

            RuleFor(x => x.Answers)
                .Must(x=>x.Distinct().Count() == x.Count)
                .WithMessage("You cannot add Dublicaited answers");

        }
    }
}
