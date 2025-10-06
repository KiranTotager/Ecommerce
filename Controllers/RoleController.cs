using ECommerce.DTO;
using ECommerce.DTOs.Auth;
using ECommerce.Interfaces.IServices.ICMSServices;
using ECommerce.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ECommerce.Controllers
{
    [Authorize(Policy = "AdminOrMangerOrHRPolicy")]
    [ApiController]
    [ApiVersion("1.0")]
    [ApiExplorerSettings(GroupName ="Authentication")]
    [Route("api/v{version:apiVersion}/[Controller]")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;
        public RoleController(IRoleService roleService)
        {
            _roleService= roleService;
        }

        [HttpPost("add/role")]
        [SwaggerOperation("this end call is to add the role")]
        public async Task<ActionResult<ResponseBase<Object>>> AddRoleAsync([FromBody] RoleDto roleDto)
        {
           await _roleService.AddAsync(roleDto.Role);
            return Ok(new ResponseBase<Object>(StatusCodes.Status201Created, "role added successfully"));
        }
        [HttpDelete("delete/role")]
        [SwaggerOperation("use this end call to delete the role")]
        public async Task<ActionResult<ResponseBase<Object>>> DeleteRoleAsync([FromBody] RoleDto roleDto)
        {
            await _roleService.DeleteAsync(roleDto.Role);
            return Ok(new ResponseBase<Object>(StatusCodes.Status200OK, "role deleted successfully"));
            
        }
        [HttpGet("get/all/roles")]
        [SwaggerOperation("use this end call to get all the roles ")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<RoleDto>> GetAllRoles()
        {
            return Ok(await _roleService.GetAllRolesAsync());
        }
    }
}
