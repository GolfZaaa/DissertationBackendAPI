using BackendAPI.Core;
using BackendAPI.DTOs.AccountDtos;
using BackendAPI.DTOs.RolesDtos;
using BackendAPI.Models;
using BackendAPI.Services.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackendAPI.Controllers
{
    public class RoleController : BaseApiController
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpGet("ShowRolesService")]
        public async Task<ActionResult> GetRoles()
        {
            return HandleResult(await _roleService.GetRoleAsync());
        }

        [HttpPost("CreateRoleService")]
        public async Task<IActionResult> CreateRole(RoleDto dto)
        {
            var validator = new RoleValidator();
            var resultvalidate = validator.Validate(dto);
            if (!resultvalidate.IsValid)
            {
                var errors = resultvalidate.Errors.Select(x => x.ErrorMessage).ToList();
                return BadRequest(new { Message = "Validation Change Email is Emtry", Errors = errors });
            }
            return HandleResult(await _roleService.CreateRoleAsync(dto));
        }


        [HttpPut("UpdateRoleService")]
        public async Task<ActionResult> Update(RoleUpdateDto dto)
        {
            var validator = new RoleUpdateValidator();
            var resultvalidate = validator.Validate(dto);
            if (!resultvalidate.IsValid)
            {
                var errors = resultvalidate.Errors.Select(x => x.ErrorMessage).ToList();
                return BadRequest(new { Message = "Validation Change Email is Emtry", Errors = errors });
            }
            return HandleResult(await _roleService.UpdateRoleAsync(dto));
        }


        [HttpDelete("DeleteRoleService")]
        public async Task<IActionResult> Delete(RoleDto dto)
        {
            var validator = new RoleValidator();
            var resultvalidate = validator.Validate(dto);
            if (!resultvalidate.IsValid)
            {
                var errors = resultvalidate.Errors.Select(x => x.ErrorMessage).ToList();
                return BadRequest(new { Message = "Validation Change Email is Emtry", Errors = errors });
            }
            return HandleResult (await _roleService.DeleteRoleAsync(dto));
        }
    }
}
