using Survey_Basket.Abstractions;

namespace Survey_Basket.Errors.Questions
{
    public class QuestionErrors
    {
        public static readonly Error QuestionNotfound =
            new("Question.Notfound", "No question was found with the given ID", StatusCodes.Status404NotFound);

        public static readonly Error DuplicateQuestionContent =
            new("Question.DuplicateContent", "Another question With the same content is already exists", StatusCodes.Status409Conflict);
    }
}
