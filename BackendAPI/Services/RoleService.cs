using BackendAPI.Core;
using BackendAPI.DTOs.RolesDtos;
using BackendAPI.Models;
using BackendAPI.Services.IServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BackendAPI.Services
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public RoleService(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task<Result<string>> CreateRoleAsync(RoleDto dto)
        {
            var identityRole = new IdentityRole
            {
                Name = dto.Name,
                NormalizedName = _roleManager.NormalizeKey(dto.Name)
            };
            var result = await _roleManager.CreateAsync(identityRole);

            if (!result.Succeeded)
            {
                return Result<string>.Failure("Create Role Failed.");
            }
            return Result<string>.Success("Create Success");
        }

        public async Task<Result<string>> DeleteRoleAsync(RoleDto dto)
        {
            var identityRole = await _roleManager.FindByNameAsync(dto.Name);
            if (identityRole == null) return Result<string>.Failure("Not Found Role");

            //ตรวจสอบมีผู้ใช้บทบาทนี้หรือไม่
            var usersInRole = await _userManager.GetUsersInRoleAsync(dto.Name);
            if (usersInRole.Count != 0) return Result<string>.Failure("User Have Role This Can't Delete.");
            var result = await _roleManager.DeleteAsync(identityRole);
            if (!result.Succeeded)
                return Result<string>.Success("Falied to Delete");

            return Result<string>.Success("Delete Success");
        }

        public async Task<Result<List<object>>> GetRoleAsync()
        {
            var roles = await _roleManager.Roles.ToListAsync();

            var usersWithRoles = roles.Select(x => new
            {
                Id = x.Id,
                Name = x.Name,
                ConcurrencyStamp = x.ConcurrencyStamp,
            }).ToList<object>();
            return Result<List<object>>.Success(usersWithRoles);
        }

        public async Task<Result<string>> UpdateRoleAsync(RoleUpdateDto dto)
        {
            var identityRole = await _roleManager.FindByNameAsync(dto.Name);

            if (identityRole == null) return Result<string>.Failure("Not Found Role ");

            identityRole.Name = dto.UpdateName;
            identityRole.NormalizedName = _roleManager.NormalizeKey(dto.UpdateName);

            var result = await _roleManager.UpdateAsync(identityRole);
            if (!result.Succeeded)
            {
                return Result<string>.Failure("Failed to Update Role.");
            }
            return Result<string>.Success("Update Role Success");
        }
    }
}
