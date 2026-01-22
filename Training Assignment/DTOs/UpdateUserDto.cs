using System.ComponentModel.DataAnnotations;

namespace Training_Assignment.DTOs
{
    /// <summary>
    /// DTO for updating a user
    /// </summary>
    public class UpdateUserDto
    {
        [Required]
        public string Name { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
