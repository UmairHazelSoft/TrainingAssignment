using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Training_Assignment.DTOs;
using Training_Assignment.Models;
using Training_Assignment.Repositories;
using Training_Assignment.Services.Interfaces;

namespace Training_Assignment.Services.Implementation
{
    public class UserService : IUserService
    {
        private readonly IRepository<User> _userRepository;
        private readonly IMapper _mapper;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly IConfiguration _configuration;
        public UserService(IRepository<User> userRepository,
                           IMapper mapper,
                           UserManager<IdentityUser> userManager,
                           IEmailSender emailSender, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _userManager = userManager;
            _emailSender = emailSender;
            _configuration = configuration;
        }

        public async Task<UserReadDto> CreateUserAsync(CreateUserDto userDto)
        {
            var identityUser = new IdentityUser
            {
                UserName = userDto.Email,
                Email = userDto.Email
            };

            var result = await _userManager.CreateAsync(identityUser);
            if (!result.Succeeded)
                throw new Exception("Failed to create Identity user");

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(identityUser);
            var confirmationLink = _configuration["ConfirmEmailUrl"] + $"?userId={identityUser.Id}&token={Uri.EscapeDataString(token)}";

            await _emailSender.SendEmailAsync(identityUser.Email, "Confirm your email", confirmationLink);

            var user = _mapper.Map<User>(userDto);
            await _userRepository.AddAsync(user);
            await _userRepository.SaveChangesAsync();

            return _mapper.Map<UserReadDto>(user);
        }

        public async Task<UserReadDto?> UpdateUserAsync(int id, UpdateUserDto userDto)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null) return null;

            _mapper.Map(userDto, user);
            _userRepository.Update(user);
            await _userRepository.SaveChangesAsync();

            return _mapper.Map<UserReadDto>(user);
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null) return false;

            _userRepository.Remove(user);
            await _userRepository.SaveChangesAsync();
            return true;
        }

        public async Task<UserReadDto?> GetUserByIdAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            return user == null ? null : _mapper.Map<UserReadDto>(user);
        }

        public async Task<IEnumerable<UserReadDto>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<UserReadDto>>(users);
        }

        public async Task<PagedResult<UserReadDto>> GetPagedUsersAsync(PaginationParams pagination)
        {
            var query = _userRepository.Query();

            if (!string.IsNullOrWhiteSpace(pagination.Search))
                query = query.Where(u => u.Name.Contains(pagination.Search) || u.Email.Contains(pagination.Search));

            query = pagination.SortBy?.ToLower() switch
            {
                "name" => pagination.SortOrder?.ToLower() == "desc" ? query.OrderByDescending(u => u.Name) : query.OrderBy(u => u.Name),
                "email" => pagination.SortOrder?.ToLower() == "desc" ? query.OrderByDescending(u => u.Email) : query.OrderBy(u => u.Email),
                _ => query.OrderBy(u => u.Id)
            };

            
            var users = await query
                .Skip((pagination.PageNumber - 1) * pagination.PageSize)
                .Take(pagination.PageSize)
                .ToListAsync();

            var totalCount = users.Count() ;

            return new PagedResult<UserReadDto>
            {
                Items = _mapper.Map<IEnumerable<UserReadDto>>(users),
                TotalCount = totalCount,
                PageNumber = pagination.PageNumber,
                PageSize = pagination.PageSize
            };
        }
    }
}
