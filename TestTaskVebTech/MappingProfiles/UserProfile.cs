using AutoMapper;
using Microsoft.AspNetCore.Server.IIS.Core;
using TestTaskVebTech.Data.DTOs;
using TestTaskVebTech.Data.Entities;
using TestTaskVebTech.Requests;
using TestTaskVebTech.Responses;
using TestTaskVebTech.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System;

namespace TestTaskVebTech.MappingProfiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserDTO>()
                .ForMember(dest => dest.Roles, 
                opt => opt.Ignore());
            CreateMap<UserDTO, User>()
                .ForMember(dest => dest.RolesUsers, 
                opt => opt.Ignore());
            CreateMap<UserDTO, UserResponse>()
                .ForMember(dest => dest.Roles, 
                opt => opt.MapFrom(src => src.Roles.Select(role => role.ToString())));
            CreateMap<UserRequest, UserDTO>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Roles,
                opt => opt.MapFrom(src => src.Roles.Select(role => ConvertStringRoleToEnumRole(role))));
            CreateMap<SortRequest, SortDTO>();
            CreateMap<PatchRequest, PatchDTO>();
            CreateMap<PaginationRequest, PaginationDTO>();
            CreateMap<UserFiltRequest, FiltDTO>();
        }
        private RoleName ConvertStringRoleToEnumRole(string roleName )
        {
            RoleName role;
            if(Enum.TryParse<RoleName>(roleName, true, out role))
            {
                return role;
            }
            else
            {
                throw new ArgumentException($"Role {roleName} does not exist");
            }
        }
    }
}
