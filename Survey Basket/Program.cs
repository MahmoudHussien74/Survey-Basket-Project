using Serilog;

namespace Survey_Basket;
public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);



        builder.Services.AddDependencies(builder.Configuration);


        builder.Services.AddExceptionHandler<ExceptionHandler>();
        builder.Services.AddProblemDetails();


        builder.Host.UseSerilog((context, configuration) =>
           configuration.ReadFrom.Configuration(context.Configuration)
        );
        
        
        
           

        var app = builder.Build();

        app.UseSerilogRequestLogging();
        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseRateLimiter();

        app.UseHttpsRedirection();

        app.UseAuthorization();


        app.MapControllers();

        app.UseExceptionHandler();

        app.Run();
    }
}
