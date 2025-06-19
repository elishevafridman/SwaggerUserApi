using Microsoft.AspNetCore.Mvc;
using Swagger_Demo.Models;

namespace Swagger_Demo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private static readonly List<StoredUser> users = new();

        [HttpPost]
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
        public IActionResult GetUsers()
        {
            return Ok(users);
        }

        [HttpPut("{id}")]
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
