using Microsoft.AspNetCore.Mvc;
using Swagger_Demo.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace Swagger_Demo.Controllers
{
    [ApiExplorerSettings(GroupName = "Users")]
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private static readonly List<StoredUser> users = new();

        [HttpPost]
        [SwaggerOperation(
            Summary = "Create a new user",
            Description = "Creates a user with name and email, and returns success message",
            Tags = new[] { "Users" })]
        [ProducesResponseType(typeof(UserResponse), 200)]
        public IActionResult CreateUser([FromBody] User user)
        {
            if (!ModelState.IsValid)
                throw new Exception("Invalid user input");

            if (users.Any(u => u.Email.Equals(user.Email, StringComparison.OrdinalIgnoreCase)))
                throw new Exception($"User with email '{user.Email}' already exists");

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
        [SwaggerOperation(
            Summary = "Get all users",
            Description = "Returns a list of all registered users",
            Tags = new[] { "Users" })]
        [ProducesResponseType(typeof(IEnumerable<StoredUser>), 200)]
        public IActionResult GetUsers()
        {
            return Ok(users);
        }

        [HttpPut("{id}")]
        [SwaggerOperation(
            Summary = "Update user by ID",
            Description = "Updates an existing user with the given ID",
            Tags = new[] { "Users" })]
        [ProducesResponseType(typeof(string), 200)]
        public IActionResult UpdateUser(string id, [FromBody] User user)
        {
            if (!ModelState.IsValid)
                throw new Exception("Invalid user input");

            var existingUser = users.FirstOrDefault(u => u.Id == id);
            if (existingUser == null)
                throw new Exception($"User with ID '{id}' not found");

            if (users.Any(u => u.Email.Equals(user.Email, StringComparison.OrdinalIgnoreCase) && u.Id != id))
                throw new Exception($"Another user with email '{user.Email}' already exists");

            existingUser.Name = user.Name;
            existingUser.Email = user.Email;

            return Ok($"User {id} updated successfully");
        }

        [HttpDelete("{id}")]
        [SwaggerOperation(
            Summary = "Delete user by ID",
            Description = "Removes a user by their unique identifier",
            Tags = new[] { "Users" })]
        [ProducesResponseType(typeof(string), 200)]
        public IActionResult DeleteUser(string id)
        {
            var userToRemove = users.FirstOrDefault(u => u.Id == id);
            if (userToRemove == null)
                throw new Exception($"User with ID '{id}' not found");

            users.Remove(userToRemove);
            return Ok($"User {id} deleted successfully");
        }
    }
}
