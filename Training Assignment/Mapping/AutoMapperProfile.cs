using AutoMapper;
using Training_Assignment.DTOs;
using Training_Assignment.Models;

namespace Training_Assignment.Mapping
{
    
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<CreateUserDto, User>();
            CreateMap<UpdateUserDto, User>();
            CreateMap<User, UserResponseDto>();
        }
    }

}
