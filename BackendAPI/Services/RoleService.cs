using BackendAPI.Core;
using BackendAPI.Data;
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
        private readonly DataContext _dataContext;

        public RoleService(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager, DataContext dataContext)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _dataContext = dataContext;
        }

        public async Task<Result<string>> CreateRoleAsync(RoleDto dto)
        {
            var checkRole = await _roleManager.Roles.FirstOrDefaultAsync(x => x.ConcurrencyStamp == dto.Name);
            if (checkRole != null)
            {
                return Result<string>.Failure("Role with the same ConcurrencyStamp already exists.");
            }

            var identityRole = new IdentityRole
            {
                Name = dto.Name,
                NormalizedName = _roleManager.NormalizeKey(dto.Name),
                ConcurrencyStamp = dto.Name,
            };
            var result = await _roleManager.CreateAsync(identityRole);

            if (!result.Succeeded)
            {
                return Result<string>.Failure("Create Role Failed.");
            }
            return Result<string>.Success("Create Success");
        }

        public async Task<Result<string>> DeleteRoleAsync(string id)
        {
            var usersWithRole = await _dataContext.UserRoles.AnyAsync(ur => ur.RoleId == id);
            if (usersWithRole)
            {
                return Result<string>.Failure("Cannot delete role because there are users assigned to it.");
            }

            var identityRole = await _dataContext.Roles.FirstOrDefaultAsync(x => x.Id == id);

            if (identityRole == null) return Result<string>.Failure("Not Found Role");

            //ตรวจสอบมีผู้ใช้บทบาทนี้หรือไม่
            var usersInRole = await _userManager.GetUsersInRoleAsync(id);
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
            var search = await _dataContext.Roles.FindAsync(dto.Id);

            if (search == null) return Result<string>.Failure("Not Found RoleId");

            if(search.ConcurrencyStamp == dto.Name) return Result<string>.Failure("The current name role of the organization.");

            if (_dataContext.Roles.Any(x => x.ConcurrencyStamp == dto.Name))
                return Result<string>.Failure("Role name have already");

            search.ConcurrencyStamp = dto.Name;
            search.NormalizedName = dto.Name;
            search.Name = dto.Name;

            await _dataContext.SaveChangesAsync();
            return Result<string>.Success("Update Role Success");
        }
    }
}
