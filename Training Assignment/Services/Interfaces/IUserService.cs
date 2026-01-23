using Training_Assignment.DTOs;
using Training_Assignment.Models;

namespace Training_Assignment.Services.Interfaces
{
    public interface IUserService
    {
        Task<UserReadDto> CreateUserAsync(CreateUserDto userDto);
        Task<UserReadDto?> UpdateUserAsync(int id, UpdateUserDto userDto);
        Task<bool> DeleteUserAsync(int id);
        Task<UserReadDto?> GetUserByIdAsync(int id);
        Task<IEnumerable<UserReadDto>> GetAllUsersAsync();
        Task<PagedResult<UserReadDto>> GetPagedUsersAsync(PaginationParams pagination);
    }
}
