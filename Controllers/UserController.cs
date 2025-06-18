using Microsoft.AspNetCore.Mvc;
using Swagger_Demo.Models;

namespace Swagger_Demo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        // רשימת המשתמשים – מדמה בסיס נתונים זמני בזיכרון
        private static readonly List<StoredUser> users = new();

        // יצירת משתמש חדש
        [HttpPost]
        [ProducesResponseType(typeof(UserResponse), 200)]
        [ProducesResponseType(400)]
        public IActionResult CreateUser([FromBody] User user)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (users.Any(u => u.Email.Equals(user.Email, StringComparison.OrdinalIgnoreCase)))
            {
                return BadRequest(new
                {
                    message = $"User with email '{user.Email}' already exists"
                });
            }

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

        // קבלת כל המשתמשים
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<StoredUser>), 200)]
        public IActionResult GetUsers()
        {
            return Ok(users);
        }

        // עדכון משתמש לפי ID
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public IActionResult UpdateUser(string id, [FromBody] User user)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingUser = users.FirstOrDefault(u => u.Id == id);
            if (existingUser == null)
                return NotFound($"User with ID {id} not found");

            // בדיקת אימייל כפול – למשתמשים אחרים בלבד
            if (users.Any(u => u.Email.Equals(user.Email, StringComparison.OrdinalIgnoreCase) && u.Id != id))
            {
                return BadRequest(new
                {
                    message = $"Another user with email '{user.Email}' already exists"
                });
            }

            existingUser.Name = user.Name;
            existingUser.Email = user.Email;

            return Ok($"User {id} updated successfully");
        }

        // מחיקת משתמש לפי ID
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(404)]
        public IActionResult DeleteUser(string id)
        {
            var userToRemove = users.FirstOrDefault(u => u.Id == id);
            if (userToRemove == null)
                return NotFound($"User with ID {id} not found");

            users.Remove(userToRemove);
            return Ok($"User {id} deleted successfully");
        }
    }
}
