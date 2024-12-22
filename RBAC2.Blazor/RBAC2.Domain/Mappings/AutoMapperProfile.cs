using AutoMapper;
using RBAC2.Database.Entities;
using RBAC2.Domain.Models;

namespace RBAC2.Domain
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, UserModel>().ReverseMap();
            CreateMap<Tasks, TaskModel>().ReverseMap();
            CreateMap<Project, ProjectModel>().ReverseMap();
        }
    }
}
