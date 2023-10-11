using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestTaskVebTech.Data.DTOs
{
    public class FiltDTO
    {
        public int? MinAge { get; set; }
        public int? MaxAge { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public List<string>? Roles { get; set; }
    }
}
