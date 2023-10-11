using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestTaskVebTech.Data.Entities
{
    public class Role
    {
        public int Id { get; set; }
        public RoleName RoleName { get; set; }
        public List<RoleUser> UsersRoles { get; set; }
    }

    public enum RoleName
    {
        User, 
        Admin, 
        Support,
        SuperAdmin
    }

}
