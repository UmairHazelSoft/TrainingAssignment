using Training_Assignment.DTOs;

namespace Training_Assignment.Services.Interfaces
{
    public interface IAuthService
    {
        Task<string?> LoginAsync(LoginDto loginDto);
    }
}
