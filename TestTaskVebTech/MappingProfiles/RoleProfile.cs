using AutoMapper;
using TestTaskVebTech.Data.DTOs;
using TestTaskVebTech.Data.Entities;
using TestTaskVebTech.Responses;

namespace TestTaskVebTech.MappingProfiles
{
    public class RoleProfile : Profile
    {
        public RoleProfile()
        {
            CreateMap<Role, RoleDTO>();
            CreateMap<RoleDTO, Role>();
            CreateMap<RoleDTO, RoleResponse>();
            CreateMap<Role, RoleResponse>();
        }

    }
}
