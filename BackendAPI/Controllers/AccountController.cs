using BackendAPI.Core;
using BackendAPI.Data;
using BackendAPI.DTOs;
using BackendAPI.DTOs.AccountDtos;
using BackendAPI.Models;
using BackendAPI.Response;
using BackendAPI.Services;
using BackendAPI.Services.IServices;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using SendGrid;
using System.Security.Claims;

namespace BackendAPI.Controllers;
public class AccountController : BaseApiController
{
    private readonly DataContext _dataContext;
    private readonly IAccountServices _accountServices;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AccountController(DataContext dataContext, IAccountServices accountServices, UserManager<ApplicationUser> userManager,
        IHttpContextAccessor httpContextAccessor)
    {
        _dataContext = dataContext;
        _accountServices = accountServices;
        _userManager = userManager;
        _httpContextAccessor = httpContextAccessor;
    }

    [HttpGet("GetAllUserService")]
    public async Task<ActionResult> ShowAllUser()
    {
        return HandleResult(await _accountServices.AllUsers());
    }

    [HttpPost("AddRoleService")]
    public async Task<IActionResult> AddRole(AddRoleUserDto dto)
    {
        var validator = new AddRoleValidator();
        var resultvalidate = validator.Validate(dto);
        if (!resultvalidate.IsValid)
        {
            var errors = resultvalidate.Errors.Select(x => x.ErrorMessage).ToList();
            return BadRequest(new { Message = "Validation Change Email is Emtry", Errors = errors });
        }

        return HandleResult(await _accountServices.AddRoleAsync(dto));
    }

    [HttpPost("loginService")]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        var LoginValidator = new LoginDtoValidator();
        var LoginValidationResult = LoginValidator.Validate(dto);

        if (!LoginValidationResult.IsValid)
        {
            var errors = LoginValidationResult.Errors.Select(error => error.ErrorMessage).ToList();
            return BadRequest(new { Message = "Validation Login Error", Errors = errors });
        }

        return HandleResult(await _accountServices.LoginAsync(dto));
    }

    [HttpPost("registerService")]
    public async Task<object> Register(RegisterDto registerDto)
    {
        var validator = new RegisterDtoValidator(_dataContext);
        var resultvalidate = validator.Validate(registerDto);

        if (!resultvalidate.IsValid)
        {
            var errors = resultvalidate.Errors.Select(x => x.ErrorMessage).ToList();
            return BadRequest(new { Message = "Validation Change Password is Emtry", Errors = errors });
        }

        return HandleResult(await _accountServices.RegisterAsync(registerDto));
    }

    [HttpDelete("DeleteUserService")]
    public async Task<ActionResult> DeleteUser(DeleteUserDto dto)
    {
        var validator = new DeleteUserValidator();
        var resultvalidate = validator.Validate(dto);

        if (!resultvalidate.IsValid)
        {
            var errors = resultvalidate.Errors.Select(x => x.ErrorMessage).ToList();
            return BadRequest(new { Message = "Validation DeleteUser", Errors = errors });
        }
        return HandleResult(await _accountServices.DeleteAsync(dto));
    }

    [HttpPost("AddProfileUser")]
    public async Task<ActionResult> AddProfileUser(AddProfileUserDto dto)
    {
        var user = await _userManager.FindByIdAsync(dto.userId);
        if(user == null)
        {
            return HandleResult(Result<string>.Failure("User Null."));
        }

        if (!string.IsNullOrEmpty(dto.firstName))
        {
            user.FirstName = dto.firstName;
        }

        if (!string.IsNullOrEmpty(dto.lastName))
        {
            user.LastName = dto.lastName;
        }

        if (!string.IsNullOrEmpty(dto.phoneNumber))
        {
            user.PhoneNumber = dto.phoneNumber;
        }

        if (dto.agencyId != null)
        {
            user.AgencyId = dto.agencyId;
        }

        await _dataContext.SaveChangesAsync();
        return HandleResult(Result<string>.Success("UpdateSuccess"));
    }

    [HttpGet("SearchUserByUserId")]
    public async Task<ActionResult> SearchUserByUserId(string userId)
    {
        var search = await _dataContext.Users.FirstOrDefaultAsync(x=>x.Id == userId);

        if(search == null)
        {
            return HandleResult(Result<string>.Failure("Not Found User"));
        }

        return HandleResult(Result<object>.Success(search));
    }


}
