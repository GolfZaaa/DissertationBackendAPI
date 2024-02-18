using BackendAPI.Core;
using BackendAPI.DTOs.RolesDtos;
using Microsoft.AspNetCore.Identity;

namespace BackendAPI.Services.IServices
{
    public interface IRoleService
    {
        Task<Result<List<object>>> GetRoleAsync();
        Task<Result<string>> CreateRoleAsync(RoleDto roleDto);
        Task<Result<string>> UpdateRoleAsync(RoleUpdateDto roleUpdateDto);
        Task<Result<string>> DeleteRoleAsync(string id);
    }
}
