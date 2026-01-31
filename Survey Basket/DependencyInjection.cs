namespace Survey_Basket
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDependencies(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddControllers();
            services.AddSwaggerServices();

            var connectionString = configuration.GetConnectionString("DefaultConnection")
             ?? throw new InvalidOperationException("Connection String 'DefaultConnection' Not found");

            services.AddDbContext<ApplicationDbContext>(option =>
            {
                option.UseSqlServer(connectionString);
            });

            services.AddAuthConfiguration(configuration);
            services.AddScoped<IAuthService,AuthService>();

            services.AddFluentValidationConfiguration()
                    .AddMapsterConfiguration();
            
            services.AddScoped<IPollService, PollService>();
            services.AddScoped<IQuestionService, QuestionService>();
            services.AddScoped<IVoteService, VoteService>();
            services.AddScoped<IResultService, ResultService>();
            return services;
        }
        public static IServiceCollection AddSwaggerServices(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            return services;
        }

        public static IServiceCollection AddFluentValidationConfiguration(this IServiceCollection services)
        {
            services.AddFluentValidationAutoValidation()
               .AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            return services;
        }
        public static IServiceCollection AddMapsterConfiguration(this IServiceCollection services)
        {

            var mappingconfig = TypeAdapterConfig.GlobalSettings;
            mappingconfig.Scan(Assembly.GetExecutingAssembly());
            services.AddSingleton<IMapper>(new Mapper(mappingconfig));
            return services;
        }
        public static IServiceCollection AddAuthConfiguration(this IServiceCollection services,IConfiguration configuration)
        {
            services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddSingleton<IJwtProvider, JwtProvider>();

            //  services.Configure<JwtOptions>(configuration.GetSection(JwtOptions.SectionName));

            services.AddOptions<JwtOptions>()
                .BindConfiguration(JwtOptions.SectionName)
                .ValidateDataAnnotations()
                .ValidateOnStart();
             
            var JwtSettings = configuration.GetSection(JwtOptions.SectionName).Get<JwtOptions>();

            services.AddAuthentication(option =>
            {
                option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
           .AddJwtBearer(option =>
           {
               option.SaveToken = true;
               option.TokenValidationParameters = new TokenValidationParameters
               {
                   ValidateIssuerSigningKey = true,
                   ValidateIssuer = true,
                   ValidateAudience = true,
                   ValidateLifetime =true,
                   IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtSettings?.Key!)),
                   ValidIssuer = JwtSettings?.Issuer,
                   ValidAudience = JwtSettings?.Audience
               };
           });
            return services;
        }
    }
}