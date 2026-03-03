namespace Survey_Basket.Mapping;

public class MappingConfiguration : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
     
        config.NewConfig<QuestionRequest, Question>()
              .Ignore(nameof(Question.Answers));

        //config.NewConfig<RegisterRequest, User>()
        //     .Map(dest => dest.UserName, src => src.Email);
    }
}
