using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Training_Assignment.DTOs;
using Training_Assignment.Services.Interfaces;

namespace Training_Assignment.Services
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
            var passwordHasher = new PasswordHasher<IdentityUser>();
            string a = passwordHasher.HashPassword(user, "aa");
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
    }


}
