using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Training_Assignment.DTOs;
using Training_Assignment.Models;
using Training_Assignment.Responses;
using Training_Assignment.Services.Interfaces;

namespace Training_Assignment.Controllers
{


    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public UsersController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        [HttpGet("all")]
        public IActionResult GetAllUsers()
        {
            var users = _userService.GetAllUsers();
            var response = _mapper.Map<List<UserResponseDto>>(users);
            return Ok(new ApiResponse<List<UserResponseDto>>(true, "Users fetched successfully", response));
        }

        [HttpGet("{id:int}")]
        public IActionResult GetUserById(int id)
        {
            var user = _userService.GetUserById(id);
            if (user == null) return NotFound(new ApiResponse<string>(false, "User not found", null));

            var response = _mapper.Map<UserResponseDto>(user);
            return Ok(new ApiResponse<UserResponseDto>(true, "User fetched successfully", response));
        }

        [HttpPost("create")]
        public IActionResult CreateUser([FromBody] CreateUserDto dto)
        {
            var user = _mapper.Map<User>(dto);
            var createdUser = _userService.CreateUser(user);
            var response = _mapper.Map<UserResponseDto>(createdUser);
            return Ok(new ApiResponse<UserResponseDto>(true, "User created successfully", response));
        }

        [HttpPut("update/{id:int}")]
        public IActionResult UpdateUser(int id, [FromBody] UpdateUserDto dto)
        {
            var userToUpdate = _mapper.Map<User>(dto);
            var updatedUser = _userService.UpdateUser(id, userToUpdate);
            if (updatedUser == null) return NotFound(new ApiResponse<string>(false, "User not found", null));

            var response = _mapper.Map<UserResponseDto>(updatedUser);
            return Ok(new ApiResponse<UserResponseDto>(true, "User updated successfully", response));
        }

        [HttpDelete("delete/{id:int}")]
        public IActionResult DeleteUser(int id)
        {
            var deleted = _userService.DeleteUser(id);
            if (!deleted) return NotFound(new ApiResponse<string>(false, "User not found", null));

            return Ok(new ApiResponse<string>(true, "User deleted successfully", null));
        }
    }

}
