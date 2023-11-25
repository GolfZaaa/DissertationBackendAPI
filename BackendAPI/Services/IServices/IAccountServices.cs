using BackendAPI.Core;
using BackendAPI.DTOs;
using BackendAPI.DTOs.AccountDtos;

namespace BackendAPI.Services.IServices
{
    public interface IAccountServices
    {
        Task<Result<string>> ChangeUserEmailAsync(ChangeUserEmailDto dto);
        Task<Result<dynamic>> AllUsers();
        Task<Result<string>> ChangeUserNameAsync(ChangeUserNameDto dto);
        Task<Result<string>> AddRoleAsync(AddRoleUserDto dto);
        Task<Result<string>> ChangePasswordAsync(ChangePasswordDto dto);
        Task<Result<object>> RegisterAsync(RegisterDto registerDto);
        Task<Result<object>> LoginAsync(LoginDto dto);
        Task<Result<string>> ConfirmEmailUserAsync(ConfirmEmailUserDto dto);
        Task<Result<string>> DeleteAsync(DeleteUserDto dto);
        Task<Result<string>> ForgetPasswordAsync(ForgetPasswordDto dto);
        Task<Result<string>> ResendOtpConfirmEmailAsync(ResendOtpConfirmEmailDto dto);
    }
}
