using System.ComponentModel.DataAnnotations;

namespace Swagger_Demo.Models
{
    /// <summary>
    /// מייצג משתמש חדש במערכת.
    /// </summary>
    public class User
    {
        /// <summary>
        /// שם המשתמש (2-50 תווים).
        /// </summary>
        [Required(ErrorMessage = "Name is required")]
        [MinLength(2, ErrorMessage = "Name must be at least 2 characters")]
        [MaxLength(50, ErrorMessage = "Name must be no more than 50 characters")]
        public string Name { get; set; }

        /// <summary>
        /// כתובת אימייל תקינה.
        /// </summary>
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }
    }
}
