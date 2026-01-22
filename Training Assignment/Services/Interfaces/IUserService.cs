using Training_Assignment.Models;

namespace Training_Assignment.Services.Interfaces
{
    public interface IUserService
    {
        List<User> GetAllUsers();
        User GetUserById(int id);
        User CreateUser(User user);
        User UpdateUser(int id, User updatedUser);
        bool DeleteUser(int id);
    }
}
