using BackendAPI.Core;
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

        [HttpGet("Show Roles")]
        public async Task<ActionResult> GetRoles()
        {
            var result = await _roleManager.Roles.ToListAsync();
            return Ok(result);

        }

        [HttpPost("CreateRole")]
        public async Task<IActionResult> CreateRole(RoleDto roleDto)
        {
            var identityRole = new IdentityRole
            {
                Name = roleDto.Name,
                NormalizedName = _roleManager.NormalizeKey(roleDto.Name)
            };
            var result = await _roleManager.CreateAsync(identityRole);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                }
                return ValidationProblem();
            }
            return StatusCode(201);
        }


        [HttpPut("UpdateRole")]
        public async Task<ActionResult> Update(RoleUpdateDto dto)
        {
            var identityRole = await _roleManager.FindByNameAsync(dto.Name);

            if (identityRole == null) return NotFound();


            identityRole.Name = dto.UpdateName;
            identityRole.NormalizedName = _roleManager.NormalizeKey(dto.UpdateName);


            var result = await _roleManager.UpdateAsync(identityRole);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                }
                return ValidationProblem();
            }
            return StatusCode(201);

        }


        [HttpDelete("Delete Role")]
        public async Task<IActionResult> Delete(RoleDto roleDto)
        {
            var identityRole = await _roleManager.FindByNameAsync(roleDto.Name);
            if (identityRole == null) return NotFound();
            //ตรวจสอบมีผู้ใช้บทบาทนี้หรือไม่
            var usersInRole = await _userManager.GetUsersInRoleAsync(roleDto.Name);
            if (usersInRole.Count != 0) return BadRequest();
            var result = await _roleManager.DeleteAsync(identityRole);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                }
                return ValidationProblem();
            }
            return StatusCode(201);
        }


    }
}
