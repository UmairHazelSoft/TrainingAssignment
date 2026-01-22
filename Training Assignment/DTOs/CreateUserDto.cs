using System.ComponentModel.DataAnnotations;

namespace Training_Assignment.DTOs
{
    /// <summary>
    /// DTO for creating a user
    /// </summary>
    public class CreateUserDto
    {
        [Required]
        public string Name { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
