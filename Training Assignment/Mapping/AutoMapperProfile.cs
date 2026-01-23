using AutoMapper;
using Training_Assignment.DTOs;
using Training_Assignment.Models;

namespace Training_Assignment.Mapping
{
    
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<CreateUserDto, User>().ReverseMap(); 
            CreateMap<UpdateUserDto, User>().ReverseMap();
            CreateMap<User, UserResponseDto>().ReverseMap();
            CreateMap<UserReadDto, UserResponseDto>().ReverseMap();
            CreateMap<User, UserReadDto>().ReverseMap();

        }
    }

}
