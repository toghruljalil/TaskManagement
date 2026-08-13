using AutoMapper;
using TaskManagementAPI.DTOs;
using TaskManagementAPI.Models;

namespace TaskManagementAPI.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<UserRegisterDto, User>();
        
        CreateMap<User, UserResponseDto>();
        
        CreateMap<ProjectCreateDto, Project>();
        
        CreateMap<Project, ProjectResponseDto>();
        
        CreateMap<TaskCreateDto, Models.Task>();
        
        CreateMap<Models.Task, TaskResponseDto>();
    }
}