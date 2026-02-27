using Microsoft.AspNetCore.RateLimiting;
using Survey_Basket.Authentication.Filters;
using System.Threading.RateLimiting;

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
            services.AddScoped<IAuthService, AuthService>();

            services.AddFluentValidationConfiguration()
                    .AddMapsterConfiguration()
                    .AddRateLimiterServices();

            services.AddScoped<IPollService, PollService>();
            services.AddScoped<IQuestionService, QuestionService>();
            services.AddScoped<IVoteService, VoteService>();
            services.AddScoped<IResultService, ResultService>();
            services.AddScoped<IRoleService, RoleService>();
            
            return services;
        }
        public static IServiceCollection AddSwaggerServices(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            return services;
        }
        public static IServiceCollection AddRateLimiterServices(this IServiceCollection services)
        {
            services.AddRateLimiter(rateLimiterOptions =>
            {

                rateLimiterOptions.AddPolicy("ipLimiter", httpContext =>
                {
                   return RateLimitPartition.GetFixedWindowLimiter(
                          partitionKey: httpContext.Connection.RemoteIpAddress?.ToString(),
                          factory:_ => new FixedWindowRateLimiterOptions
                          {
                              PermitLimit = 2,
                              Window = TimeSpan.FromSeconds(20),
                          }
                        );
                });
                rateLimiterOptions.AddPolicy("userLimiter", httpContext =>
                {
                   return RateLimitPartition.GetFixedWindowLimiter(
                          partitionKey: httpContext.User.GetUserId(),
                          factory:_ => new FixedWindowRateLimiterOptions
                          {
                              PermitLimit = 2,
                              Window = TimeSpan.FromSeconds(20),
                          }
                        );
                });

                rateLimiterOptions.AddConcurrencyLimiter("concurrency", option =>
                {
                    option.PermitLimit = 1000;
                    option.QueueLimit = 100;
                    option.QueueProcessingOrder = System.Threading.RateLimiting.QueueProcessingOrder.OldestFirst;
                });
            });


            //rateLimiterOption.AddTokenBucketLimiter("tokenBucket", option =>
            //{
            //    option.TokenLimit = 2;
            //    option.QueueLimit = 1;
            //    option.QueueProcessingOrder = System.Threading.RateLimiting.QueueProcessingOrder.OldestFirst;
            //    option.ReplenishmentPeriod = TimeSpan.FromSeconds(10);
            //    option.TokensPerPeriod = 2;
            //    option.AutoReplenishment = true;

            //});

            //rateLimiterOption.AddFixedWindowLimiter("fixedWindow", option =>
            //{
            //    option.PermitLimit = 2;
            //    option.QueueLimit = 1;
            //    option.QueueProcessingOrder = System.Threading.RateLimiting.QueueProcessingOrder.OldestFirst;
            //    option.Window = TimeSpan.FromSeconds(20);

            //});

            //rateLimiterOption.AddSlidingWindowLimiter("slidingWindow", option =>
            //{
            //    option.PermitLimit = 2;
            //    option.QueueLimit = 1;
            //    option.QueueProcessingOrder = System.Threading.RateLimiting.QueueProcessingOrder.OldestFirst;
            //    option.Window = TimeSpan.FromSeconds(20);
            //    option.SegmentsPerWindow = 2;

            //});


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
            services.AddIdentity<User, ApplicationRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddSingleton<IJwtProvider, JwtProvider>();
            services.AddTransient<IAuthorizationHandler, PermissionAuthorizationHandler>();
            services.AddTransient<IAuthorizationPolicyProvider , PermissionAuthorizationPolicyProvider>();
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