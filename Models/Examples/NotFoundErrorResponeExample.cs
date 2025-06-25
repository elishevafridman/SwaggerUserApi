using Swagger_Demo.Models;
using Swashbuckle.AspNetCore.Filters;

namespace Swagger_Demo.Examples;

public class NotFoundErrorResponseExample : IExamplesProvider<ErrorResponse>
{
    public ErrorResponse GetExamples()
    {
        return new ErrorResponse
        {
            StatusCode = 404,
            Message = "User with ID not found",
            Details = "The requested user does not exist in the system"
        };
    }
}
