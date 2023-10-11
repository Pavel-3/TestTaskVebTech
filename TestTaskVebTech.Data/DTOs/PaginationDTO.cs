using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestTaskVebTech.Data.DTOs
{
    public class PaginationDTO
    {
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
    }
}
