using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestTaskVebTech.Data.Entities;

namespace TestTaskVebTech.Data.DTOs
{
    public class RoleDTO
    {
        public int Id {get; set;}
        public RoleName RoleName { get; set; }
    }
}
