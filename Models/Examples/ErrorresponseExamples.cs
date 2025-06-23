using Swagger_Demo.Models;
using Swashbuckle.AspNetCore.Filters;

namespace Swagger_Demo.Examples
{
    public class ErrorResponseExample : IExamplesProvider<ErrorResponse>
    {
        public ErrorResponse GetExamples()
        {
            return new ErrorResponse
            {
                StatusCode = 400,
                Message = "User with email already exists",
                Details = "Duplicate email constraint violation"
            };
        }
    }
}
