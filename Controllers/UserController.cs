using Microsoft.AspNetCore.Mvc;
using Swagger_Demo.Models;
using Swagger_Demo.Examples;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;

namespace Swagger_Demo.Controllers
{
    [ApiExplorerSettings(GroupName = "Users")]
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private static readonly List<StoredUser> users = new();

        // POST: Create new user
        [HttpPost]
        [SwaggerOperation(Summary = "Create a new user", Tags = new[] { "Users" })]
        [ProducesResponseType(typeof(UserResponse), 200)]
        [SwaggerResponseExample(200, typeof(UserResponseExample))]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        [SwaggerResponseExample(400, typeof(ErrorResponseExample))]
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

            return Ok(new UserResponse
            {
                Id = newUser.Id,
                Message = $"User {user.Name} created successfully"
            });
        }

        // GET: Get all users
        [HttpGet]
        [SwaggerOperation(Summary = "Get all users", Tags = new[] { "Users" })]
        [ProducesResponseType(typeof(IEnumerable<StoredUser>), 200)]
        [SwaggerResponseExample(200, typeof(UserListExample))]
        public IActionResult GetUsers()
        {
            return Ok(users);
        }

        // PUT: Update user by ID
        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "Update user by ID", Tags = new[] { "Users" })]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        [SwaggerResponseExample(400, typeof(ErrorResponseExample))]
        [ProducesResponseType(typeof(ErrorResponse), 404)]
        [SwaggerResponseExample(404, typeof(ErrorResponseExample))]
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

        // DELETE: Delete user by ID
        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Delete user by ID", Tags = new[] { "Users" })]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(typeof(ErrorResponse), 404)]
        [SwaggerResponseExample(404, typeof(ErrorResponseExample))]
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
