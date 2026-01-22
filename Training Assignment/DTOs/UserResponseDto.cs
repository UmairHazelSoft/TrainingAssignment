namespace Training_Assignment.DTOs
{
    /// <summary>
    /// DTO for returning user details
    /// </summary>
    public class UserResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
