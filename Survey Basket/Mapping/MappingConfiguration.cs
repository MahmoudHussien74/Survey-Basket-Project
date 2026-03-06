using Survey_Basket.Contracts.Users;

namespace Survey_Basket.Mapping;

public class MappingConfiguration : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
     
        config.NewConfig<QuestionRequest, Question>()
              .Ignore(nameof(Question.Answers));

        config.NewConfig<UserProfileResponse,User>();


        config.NewConfig<(User user, IList<string> roles), UserResponse>()
            .Map(dest=>dest,src=>src.user)
            .Map(dest=>dest.Roles,src=>src.roles);


        config.NewConfig<CreateUserRequest, User>()
            .Map(dest => dest.UserName, src => src.Email)
            .Map(dest => dest.EmailConfirmed, src =>true);

        config.NewConfig<UpdateUserRequest, User>()
            .Map(dest => dest.UserName, src => src.Email)
            .Map(dest => dest.NormalizedUserName, src =>src.Email.ToUpper());


    }
}
