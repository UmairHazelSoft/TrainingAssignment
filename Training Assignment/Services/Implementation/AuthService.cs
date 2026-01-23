using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Training_Assignment.DTOs;
using Training_Assignment.Services.Interfaces;

namespace Training_Assignment.Services.Implementation
{

    /// <summary>
    /// Handles authentication and JWT token generation.
    /// </summary>
    public class AuthService : IAuthService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _configuration;

        public AuthService(UserManager<IdentityUser> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        public async Task<string?> LoginAsync(LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user == null) return null;
            var isValid = await _userManager.CheckPasswordAsync(user, loginDto.Password);
            if (!isValid) return null;

            // Generate JWT
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, user.UserName!),
                    new Claim(ClaimTypes.Email, user.Email!)
                }),
                Expires = DateTime.UtcNow.AddMinutes(double.Parse(_configuration["Jwt:DurationInMinutes"])),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        // ------------------------
        // Email confirmation
        // ------------------------
        public async Task<(bool Success, string Message)> ConfirmEmailAsync(string userId, string token)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return (false, "User not found");

            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
                return (true, "Email confirmed successfully. You can now set your password.");

            return (false, "Invalid token");
        }

        // ------------------------
        // Set password
        // ------------------------
        public async Task<(bool Success, string Message, List<string>? Errors)> SetPasswordAsync(SetPasswordDto model)
        {
            if (model == null || string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.NewPassword))
                return (false, "Email and new password are required.", null);

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
                return (false, "User not found.", null);

            if (!user.EmailConfirmed)
                return (false, "Email is not confirmed yet.", null);

            var hasPassword = await _userManager.HasPasswordAsync(user);
            if (hasPassword)
                return (false, "Password already set. You can login.", null);

            var result = await _userManager.AddPasswordAsync(user, model.NewPassword);
            if (!result.Succeeded)
            {
                return (false, "Failed to set password.", result.Errors.Select(e => e.Description).ToList());
            }

            return (true, "Password set successfully. You can now login.", null);
        }
    }


}
