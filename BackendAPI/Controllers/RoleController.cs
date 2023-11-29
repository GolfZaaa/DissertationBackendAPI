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
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IRoleService _roleService;

        public RoleController(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager,
            IRoleService roleService)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _roleService = roleService;
        }

        [HttpGet("Show Roles Service!")]
        public async Task<ActionResult> GetRoles()
        {
            return HandleResult(await _roleService.GetRoleAsync());
        }

        [HttpPost("CreateRole Service!")]
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


        [HttpPut("UpdateRole Service!")]
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


        [HttpDelete("Delete Role Service!")]
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
