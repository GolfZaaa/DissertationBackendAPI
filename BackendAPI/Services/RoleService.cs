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

        public Task<Result<string>> CreateRoleAsync(RoleDto roleDto)
        {
            throw new NotImplementedException();
        }

        public Task<Result<string>> DeleteRoleAsync(RoleDto roleDto)
        {
            throw new NotImplementedException();
        }

        public async Task<Result<List<object>>> GetRoleAsync()
        {
            var roles = await _roleManager.Roles.ToListAsync();

            var usersWithRoles = roles.Select(x => new
            {
                Name = x.Name,
            }).ToList<object>();
            return Result<List<object>>.Success(usersWithRoles);
        }

        public Task<Result<string>> UpdateRoleAsync(RoleUpdateDto roleUpdateDto)
        {
            throw new NotImplementedException();
        }
    }
}
