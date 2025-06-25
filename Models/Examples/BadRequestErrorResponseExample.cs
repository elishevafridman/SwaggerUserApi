using Swagger_Demo.Models;
using Swashbuckle.AspNetCore.Filters;

namespace Swagger_Demo.Examples;

public class BadRequestErrorResponseExample : IExamplesProvider<ErrorResponse>
{
    public ErrorResponse GetExamples()
    {
        return new ErrorResponse
        {
            StatusCode = 400,
            Message = "User with email 'alice@example.com' already exists",
            Details = "Duplicate email constraint violation"
        };
    }
}
