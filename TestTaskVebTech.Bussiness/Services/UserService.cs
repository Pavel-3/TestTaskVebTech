using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Data;
using TestTaskVebTech.Bussiness.Abstractions;
using TestTaskVebTech.Data;
using TestTaskVebTech.Data.DTOs;
using TestTaskVebTech.Data.Entities;
using System.Linq.Dynamic.Core;
using Microsoft.EntityFrameworkCore.DynamicLinq;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Runtime.ExceptionServices;
using System.Linq;

namespace TestTaskVebTech.Bussiness.Services
{
    public class UserService : IUserService
    {
        private readonly UserListContext _context;
        private readonly IMapper _mapper;
        public UserService(UserListContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }
        public async Task<List<UserDTO>> GetAllUsersAsync()
        {
            var users = await _context.Users
                .Select(user => _mapper.Map<UserDTO>(user)).ToListAsync();
            return users;
        }

        public async Task<List<UserDTO>> GetUsersByPageAsync(PaginationDTO pagination,
            FiltDTO filt,
            SortDTO sort)
        {
            var properties = typeof(User).GetProperties();

            var usersQuery = _context.Users.AsQueryable();
            if (filt.MinAge != null)
                usersQuery = usersQuery.Where(user => user.Age >= filt.MinAge);
            if (filt.MaxAge != null)
                usersQuery = usersQuery.Where(user => (user.Age <= filt.MaxAge));
            if (!string.IsNullOrEmpty( filt.UserName))
                usersQuery = usersQuery.Where(user => user.Name.Contains(filt.UserName));
            if (!string.IsNullOrEmpty(filt.Email))
                usersQuery = usersQuery.Where(user => user.Email.Contains(filt.Email));
            var roleEnums = new List<RoleName>();
            if (filt.Roles != null && filt.Roles.Any())
            {
                usersQuery = usersQuery.Where(u => u.RolesUsers
                .Any(ru => filt.Roles
                .Contains(ru.Role.RoleName.ToString())));
            }

            if (!string.IsNullOrEmpty(sort.PropertyName))
            {
                var newusers = sort.IsAscending ?
                    usersQuery.OrderBy(sort.PropertyName) :
                    usersQuery.OrderBy(sort.PropertyName + "desc");
            }
            var users = await usersQuery
                .Include(user => user.RolesUsers)
                .Skip((pagination.CurrentPage - 1) * pagination.PageSize)
                .Take(pagination.PageSize).ToListAsync();
            var userDTOs = new List<UserDTO>();
            foreach (var user in users)
            {
                var roles = _context.Roles
                .Where(role => user.RolesUsers
                .Select(role => role.RoleId)
                .Contains(role.Id))
                .Select(role => role.RoleName);
                var userDTO = _mapper.Map<UserDTO>(user);
                userDTO.Roles = await roles.ToListAsync();
                userDTOs.Add(userDTO);
            }
            return userDTOs;
        }

        public async Task<UserDTO?> GetUserByIdAsync(int id)
        {
            var user = await _context.Users
                .Include(user => user.RolesUsers)
                .FirstOrDefaultAsync(user => user.Id == id);
            if (user == null)
                return null;
            var roles = GetUserRoles(user);

            var userDTO = _mapper.Map<UserDTO>(user);
            userDTO.Roles = roles.ToList();
            return userDTO;
        }
        private IQueryable<RoleName> GetUserRoles(User user)
        {
            var roles = _context.Roles
                .Where(role => user.RolesUsers
                .Select(role => role.RoleId)
                .Contains(role.Id))
                .Select(role => role.RoleName);
            return roles;
        }
        public async Task<UserDTO?> AddRoleToUserAsync(int id, string roleString)
        {
            if (!Enum.TryParse<RoleName>(roleString, true, out var roleName))
            {
                throw new ArgumentException($"Role {roleString} not found");
            }
            var user = await _context.Users.Include(user => user.RolesUsers)
                .FirstOrDefaultAsync(user => user.Id == id);
            if (user == null)
            {
                throw new ArgumentException("User not found");
            }
            var role = _context.Roles.FirstOrDefault(role => role.RoleName == roleName);
            if (role == null)
            {
                throw new DataException("Role not found");
            }
            var roleUser = user.RolesUsers.FirstOrDefault(roleUser => roleUser.RoleId == role.Id);
            bool isRoleAssign = (roleUser != null);
            if (isRoleAssign)
            {
                throw new InvalidExpressionException($"The role {roleName} is already assign to the User");
            }
            await _context.RoleUsers.AddAsync(new RoleUser()
            {
                UserId = user.Id,
                RoleId = role.Id
            });
            await _context.SaveChangesAsync();
            return _mapper.Map<UserDTO>(user);
        }
        public async Task<UserDTO> AddUserAsync(UserDTO userDTO)
        {
            EntityEntry<User>? userEntry = null;
            if (isDataValid(userDTO))
            {
                userEntry = await _context.Users.AddAsync(_mapper.Map<User>(userDTO));
                _context.SaveChanges();
                foreach (var roleName in userDTO.Roles)
                {
                    var role = await _context.Roles.FirstAsync(role => role.RoleName == roleName);
                    await _context.RoleUsers.AddAsync(new RoleUser()
                    {
                        RoleId = role.Id,
                        UserId = userEntry.Entity.Id
                    });
                }
                await _context.SaveChangesAsync();
            }
            return _mapper.Map<UserDTO>(userEntry.Entity);
        }
        public async Task PatchUserAsync(int id, List<PatchDTO> patchDtos)
        {
            var entity =
                await _context.Users.FirstOrDefaultAsync(ent => ent.Id == id);
            if (entity == null)
            {
                throw new ArgumentException("User not found");
            }
            foreach (var patchDTO in patchDtos)
            {
                switch (patchDTO.PropertyName)
                {
                    case "Email":
                        patchDTO.PropertyValue = patchDTO.PropertyValue.ToString();
                        break;
                    case "Age":
                        if (int.TryParse(patchDTO?.PropertyValue?.ToString(), out var age))
                        {
                            patchDTO.PropertyValue = age;
                        }
                        break;
                    case "Name":
                        patchDTO.PropertyValue = patchDTO.PropertyValue.ToString();
                        break;
                }
            }

            var nameValuePairProperties = patchDtos.ToDictionary
            (k => k.PropertyName,
                v => v.PropertyValue);

            var dbEntityEntry = _context.Entry(entity);
            dbEntityEntry.CurrentValues.SetValues(nameValuePairProperties);
            dbEntityEntry.State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
        public async Task UpdateUserAsync(UserDTO user)
        {
            _context.Users.Update(_mapper.Map<User>(user));
        }
        public async Task RemoveUserAsync(int id)
        {
            var entity =
                await _context.Users.FirstOrDefaultAsync(ent => ent.Id == id);
            if (entity == null)
                throw new ArgumentException($"User not with id {id} not found");
            _context.Users.Remove(entity);
            await _context.SaveChangesAsync();
        }
        private bool isDataValid(UserDTO userDTO)
        {
            var properties = typeof(UserDTO).GetProperties();
            foreach (var property in properties)
            {
                var propertyType = property.PropertyType;
                bool isPropertyNullableAndNotId = (Nullable.GetUnderlyingType(propertyType) != null) && property.Name != "Id";
                if (isPropertyNullableAndNotId)
                {
                    var value = property.GetValue(userDTO);
                    if (value == null)
                    {
                        throw new ArgumentException($"Value {property.Name} can't be null");
                    }
                }
            }
            bool isEmailUnique = (_context.Users.FirstOrDefault(user => user.Email == userDTO.Email)) == null;
            if (!isEmailUnique)
                throw new ArgumentException($"Email {userDTO.Email} is already used");
            bool isAgeNegativeNumber = userDTO.Age < 0;
            if (isAgeNegativeNumber)
                throw new ArgumentException($"Age must be positive number");
            return true;
        }
    }
}
