using Microsoft.AspNetCore.Mvc;
using Training_Assignment.DTOs;

namespace Training_Assignment.Services.Interfaces
{
    public interface IAuthService
    {
        Task<string?> LoginAsync(LoginDto loginDto);
        Task<(bool Success, string Message, List<string>? Errors)> SetPasswordAsync(SetPasswordDto model);
        Task<(bool Success, string Message)> ConfirmEmailAsync(string userId, string token);
    }
}
