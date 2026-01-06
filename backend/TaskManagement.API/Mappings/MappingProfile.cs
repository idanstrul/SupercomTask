using AutoMapper;
using TaskManagement.API.DTOs;
using TaskManagement.Core.Models;

namespace TaskManagement.API.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // TaskItem mappings
        CreateMap<TaskItem, TaskItemDto>()
            .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.User != null ? new UserDto
            {
                Id = src.User.Id,
                FullName = src.User.FullName,
                Telephone = src.User.Telephone,
                Email = src.User.Email
            } : null));

        CreateMap<CreateTaskItemDto, TaskItem>();
        CreateMap<UpdateTaskItemDto, TaskItem>();

        // User mappings
        CreateMap<User, UserDto>();
        CreateMap<CreateUserDto, User>();
        CreateMap<UpdateUserDto, User>();
    }
}
