using Microsoft.AspNetCore.Mvc;
using Swagger_Demo.Examples;
using Swagger_Demo.Models;
using Swagger_Demo.Models.Exceptions;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;

namespace Swagger_Demo.Controllers;

[ApiController]
[Route("[controller]")]
[ApiExplorerSettings(GroupName = "Users")]
public class UserController : ControllerBase
{
    private static readonly List<StoredUser> users = new();

    [HttpPost]
    [SwaggerOperation(Summary = "Create a new user")]
    [ProducesResponseType(typeof(UserResponse), 200)]
    [ProducesResponseType(typeof(ErrorResponse), 400)]
    [SwaggerResponseExample(400, typeof(BadRequestErrorResponseExample))]
    public IActionResult CreateUser([FromBody] User user)
    {
        if (!ModelState.IsValid)
            throw new BadRequestException("Invalid user data");

        if (users.Any(u => u.Email.Equals(user.Email, StringComparison.OrdinalIgnoreCase)))
            throw new BadRequestException($"User with email '{user.Email}' already exists");

        var newUser = new StoredUser
        {
            Id = Guid.NewGuid().ToString(),
            Name = user.Name,
            Email = user.Email
        };

        users.Add(newUser);

        var response = new UserResponse
        {
            Id = newUser.Id,
            Message = $"User {user.Name} created successfully"
        };

        return Ok(response);
    }

    [HttpGet]
    [SwaggerOperation(Summary = "Get all users")]
    [ProducesResponseType(typeof(IEnumerable<StoredUser>), 200)]
    public IActionResult GetUsers()
    {
        return Ok(users);
    }

    [HttpPut("{id}")]
    [SwaggerOperation(Summary = "Update user by ID")]
    [ProducesResponseType(typeof(string), 200)]
    [ProducesResponseType(typeof(ErrorResponse), 400)]
    [ProducesResponseType(typeof(ErrorResponse), 404)]
    [SwaggerResponseExample(400, typeof(BadRequestErrorResponseExample))]
    [SwaggerResponseExample(404, typeof(NotFoundErrorResponseExample))]
    public IActionResult UpdateUser(string id, [FromBody] User user)
    {
        if (!ModelState.IsValid)
            throw new BadRequestException("Invalid user data");

        var existingUser = users.FirstOrDefault(u => u.Id == id);
        if (existingUser == null)
            throw new NotFoundException($"User with ID {id} not found");

        if (users.Any(u => u.Email.Equals(user.Email, StringComparison.OrdinalIgnoreCase) && u.Id != id))
            throw new BadRequestException($"Another user with email '{user.Email}' already exists");

        existingUser.Name = user.Name;
        existingUser.Email = user.Email;

        return Ok($"User {id} updated successfully");
    }

    [HttpDelete("{id}")]
    [SwaggerOperation(Summary = "Delete user by ID")]
    [ProducesResponseType(typeof(string), 200)]
    [ProducesResponseType(typeof(ErrorResponse), 404)]
    [SwaggerResponseExample(404, typeof(NotFoundErrorResponseExample))]
    public IActionResult DeleteUser(string id)
    {
        var userToRemove = users.FirstOrDefault(u => u.Id == id);
        if (userToRemove == null)
            throw new NotFoundException($"User with ID {id} not found");

        users.Remove(userToRemove);
        return Ok($"User {id} deleted successfully");
    }
}
