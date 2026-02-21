namespace Survey_Basket;
public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);



        builder.Services.AddDependencies(builder.Configuration);


        builder.Services.AddExceptionHandler<ExceptionHandler>();
        builder.Services.AddProblemDetails();
        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        app.UseHttpsRedirection();

        app.UseAuthorization();


        app.MapControllers();

        app.UseExceptionHandler();

        app.Run();
    }
}
