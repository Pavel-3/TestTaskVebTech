using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using TestTaskVebTech.Data;
using TestTaskVebTech.Responses;

namespace TestTaskVebTech.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : Controller
    {
        private readonly UserListContext _context;
        private readonly IMapper _mapper;
        public RoleController(UserListContext context, IMapper mapper) 
        {
            _context = context;
            _mapper = mapper;
        }
        [HttpGet]
        [Route("GetRoles")]
        public ActionResult GetAllRoles ()
        {
            var roles = _context.Roles.Select(role => _mapper.Map<RoleResponse>(role));
            return Ok(roles);
        }
    }
}
