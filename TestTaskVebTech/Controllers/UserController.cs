using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using TestTaskVebTech.Bussiness.Abstractions;
using TestTaskVebTech.Data.DTOs;
using TestTaskVebTech.Requests;
using TestTaskVebTech.Responses;

namespace TestTaskVebTech.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private IConfiguration _configuration;
        private IMapper _mapper;
        private IUserService _userService;
        public UserController(IConfiguration configuration, IMapper mapper, IUserService userService)
        {
            _configuration = configuration;
            _mapper = mapper;
            _userService = userService;
        }
        [HttpGet]
        [Route("GetUsers/")]
        public async Task<IActionResult> GetUsers([FromQuery] PaginationRequest pagination, 
            [FromQuery]UserFiltRequest? userFiltRequest,
            [FromQuery]SortRequest? sortRequest)
        {
            try
            {
                userFiltRequest = userFiltRequest?? new UserFiltRequest();
                sortRequest = sortRequest?? new SortRequest();
                var users = (await _userService.GetUsersByPageAsync(_mapper.Map<PaginationDTO>(pagination),
                    _mapper.Map<FiltDTO>(userFiltRequest),
                    _mapper.Map<SortDTO>(sortRequest)));
                var usersRequest = users
                .Select(user => _mapper.Map<UserResponse>(user)).ToList();
                if (users.Any())
                    return Ok(users);
                return NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }

        [HttpGet]
        [Route("GetUser/{Id}")]
        public async Task<IActionResult> GetUser([FromRoute]int Id)
        {
            try
            {
                var user = _mapper.Map<UserResponse>(await _userService.GetUserByIdAsync(Id));
                if (user == null)
                    return NotFound();
                return Ok(user);
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpPatch]
        [Route("AddRoleToUser/{id}")]
        public async Task<IActionResult> AddRoleToUser([FromRoute]int id, [FromBody]string role)
        {
            try
            {
                await _userService.AddRoleToUserAsync(id, role);
                return NoContent();
            }
            catch(InvalidExpressionException ex)
            {
                return BadRequest(ex.Message);
            }
            catch(DataException ex) 
            {
                return NotFound(ex.Message);
            }
            catch(ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPatch]
        [Route("PatchUser/{id}")]
        public async Task<IActionResult> PatchUser([FromRoute] int id, [FromBody]params PatchRequest[] patchRequests)
        {
            try
            {
                await _userService
                    .PatchUserAsync(id,
                    patchRequests.Select(patch => _mapper
                    .Map<PatchDTO>(patch))
                    .ToList());
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpPut]
        [Route("UpdateUser")]
        public async Task<IActionResult> UpdateUser([FromQuery]int id,[FromBody]UserRequest userRequest)
        {
            try
            {
                var user = _userService.GetUserByIdAsync(id);
                if (user == null)
                    return NotFound();
                var userDTO = _mapper.Map<UserDTO>(userRequest);
                userDTO.Id = id;
                await _userService.UpdateUserAsync(userDTO);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpPost]
        [Route("AddUser")]
        public async Task<IActionResult> AddUser([FromBody] UserRequest userRequest)
        {
            try
            {
                var user = await _userService.AddUserAsync(_mapper.Map<UserDTO>(userRequest));
                return Created($"api/GetUser/{user.Id}", null);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete]
        [Route("DeleteUser/{id}")]
        public async Task<IActionResult> DeleteUser([FromRoute]int id)
        {
            try
            {
            await _userService.RemoveUserAsync(id);
            return NoContent();
            }
            catch(ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
