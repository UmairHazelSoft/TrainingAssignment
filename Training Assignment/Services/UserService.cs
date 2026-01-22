using Training_Assignment.Models;
using Training_Assignment.Services.Interfaces;

namespace Training_Assignment.Services
{
    

    /// <summary>
    /// User service implementing CRUD operations
    /// </summary>
    public class UserService : IUserService
    {
        private readonly List<User> _users = new();
        private int count = 0;

        public List<User> GetAllUsers() => _users;

        public User GetUserById(int id) => _users.FirstOrDefault(u => u.Id == id);

        public User CreateUser(User user)
        {
            user.Id = count + 1; // simple auto-increment
            count++;
            _users.Add(user);
            return user;
        }

        public User UpdateUser(int id, User updatedUser)
        {
            var user = _users.FirstOrDefault(u => u.Id == id);
            if (user == null) return null;

            user.Name = updatedUser.Name;
            user.Email = updatedUser.Email;
            user.Password = updatedUser.Password;

            return user;
        }

        public bool DeleteUser(int id)
        {
            var user = _users.FirstOrDefault(u => u.Id == id);
            if (user == null) return false;
            _users.Remove(user);
            return true;
        }
    }

}
