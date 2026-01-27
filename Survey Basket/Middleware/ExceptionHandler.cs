namespace Survey_Basket.Middleware
{
    using Microsoft.AspNetCore.Diagnostics;

    namespace Survey_Basket.Middleware
    {
        public class ExceptionHandler(ILogger<ExceptionHandler> logger) : IExceptionHandler
        {
            private readonly ILogger<ExceptionHandler> _logger = logger;

            public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
            {
                _logger.LogError(exception, "something want wrong :{Message}",exception.Message);

                var problemDetails = new ProblemDetails
                {
                    Title = "InternalServerError",
                    Status = StatusCodes.Status500InternalServerError,
                    Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.6.1"
                };

                httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;

                await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

                return true;
            }
        }

    }
}
