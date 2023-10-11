using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TestTaskVebTech.Data.DTOs;
using TestTaskVebTech.Data.Entities;

namespace TestTaskVebTech.Bussiness.Abstractions
{
    public interface IUserService
    {
        public Task<List<UserDTO>> GetAllUsersAsync();
        public Task<List<UserDTO>> GetUsersByPageAsync(PaginationDTO pagination,
            FiltDTO filt,
            SortDTO sort);
        public Task<UserDTO> GetUserByIdAsync(int id);
        public Task<UserDTO> AddUserAsync(UserDTO userDTO);
        public Task<UserDTO?> AddRoleToUserAsync(int id, string roleString);
        public Task PatchUserAsync(int id, List<PatchDTO> patchDtos);
        public Task UpdateUserAsync(UserDTO user);
        public Task RemoveUserAsync(int id);
    }
}
