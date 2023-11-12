using BackendAPI.Controllers;
using BackendAPI.DTOs.AccountDtos;

namespace BackendAPI.Services
{
    public interface IAccountService 
    {
        Task<Object> ChangeUserEmailAsync(ChangeUserEmailDto dto);
    }
}
