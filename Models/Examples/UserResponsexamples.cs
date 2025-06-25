using Swagger_Demo.Models;
using Swashbuckle.AspNetCore.Filters;

namespace Swagger_Demo.Examples;

public class UserResponseExample : IExamplesProvider<UserResponse>
{
    public UserResponse GetExamples()
    {
        return new UserResponse
        {
            Id = "c45e2f98-9340-45ab-a40a-8cc98003d716",
            Message = "User Alice created successfully"
        };
    }
}
