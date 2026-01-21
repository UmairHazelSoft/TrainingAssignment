using Microsoft.AspNetCore.Mvc;
using Training_Assignment.Models;

namespace Training_Assignment.Controllers
{
    /// <summary>
    /// Handles CRUD operations for users.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private static readonly List<User> Users = new();
        private static int _nextId = 1;

        [HttpGet]
        public IActionResult GetAllUsers() => Ok(Users);

        [HttpGet("{id}")]
        public IActionResult GetUserById(int id)
        {
            var user = Users.FirstOrDefault(u => u.Id == id);
            if (user == null) return NotFound("User not found.");
            return Ok(user);
        }

        [HttpPost]
        public IActionResult CreateUser(User user)
        {
            user.Id = _nextId++;
            Users.Add(user);
            return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, user);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateUser(int id, User updatedUser)
        {
            var user = Users.FirstOrDefault(u => u.Id == id);
            if (user == null) return NotFound("User not found.");

            user.Name = updatedUser.Name;
            user.Email = updatedUser.Email;
            user.Password = updatedUser.Password;

            return Ok(user);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteUser(int id)
        {
            var user = Users.FirstOrDefault(u => u.Id == id);
            if (user == null) return NotFound("User not found.");

            Users.Remove(user);
            return NoContent();
        }
    }
}
