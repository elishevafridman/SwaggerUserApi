using System.ComponentModel.DataAnnotations;

namespace Swagger_Demo.Models
{
    /// <summary>
    /// מייצג משתמש כפי שהוא נשמר במערכת (כולל מזהה).
    /// </summary>
    public class StoredUser
    {
        /// <summary>
        /// מזהה ייחודי של המשתמש.
        /// </summary>
        [Required]
        public string Id { get; set; }

        /// <summary>
        /// שם המשתמש.
        /// </summary>
        [Required]
        [MinLength(2, ErrorMessage = "Name must be at least 2 characters")]
        [MaxLength(50, ErrorMessage = "Name must be no more than 50 characters")]
        public string Name { get; set; }

        /// <summary>
        /// כתובת האימייל של המשתמש.
        /// </summary>
        [Required]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }
    }
}
