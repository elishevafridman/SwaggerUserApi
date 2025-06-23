using System.ComponentModel.DataAnnotations;

namespace Swagger_Demo.Models
{
    /// <summary>
    /// תגובת שרת המכילה מזהה והודעה.
    /// </summary>
    public class UserResponse
    {
        /// <summary>
        /// מזהה הישות (לרוב מזהה המשתמש).
        /// </summary>
        [Required]
        public string Id { get; set; }

        /// <summary>
        /// הודעה כללית לגבי הפעולה (למשל הצלחה או שגיאה).
        /// </summary>
        [Required]
        [MinLength(1)]
        public string Message { get; set; }
    }
}
