using System.ComponentModel.DataAnnotations;

namespace Training_Assignment.DTOs
{
    public class SetPasswordDto
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string NewPassword { get; set; }
    }

}
