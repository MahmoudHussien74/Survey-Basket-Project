namespace Survey_Basket.Abstractions
{
    public static class ResultsExtentions
    {
        public static ObjectResult ToProblem(this Result result)
        {
            if (result.IsSuccess)
                throw new InvalidOperationException();



            var problem = Results.Problem(statusCode: result.Error.statusCode);

            var problemDetails = problem.GetType().GetProperty(nameof(ProblemDetails))!.GetValue(problem) as ProblemDetails;

            problemDetails!.Extensions = new Dictionary<string, object?>
            {
                {
                    "errors",
                    new[]
                    {
                        new
                        {
                        result.Error.Code,
                        result.Error.Message
                        }
                    }
                }
            };
            return new ObjectResult(problemDetails);
        }
    }
}
