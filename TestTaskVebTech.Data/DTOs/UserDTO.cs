using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestTaskVebTech.Data.Entities;

namespace TestTaskVebTech.Data.DTOs
{
    public class UserDTO
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public int? Age { get; set; }
        public List<RoleName> Roles { get; set; }
    }
}
