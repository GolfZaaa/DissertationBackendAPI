using BackendAPI.Core;
using BackendAPI.Data;
using BackendAPI.DTOs;
using BackendAPI.DTOs.AccountDtos;
using BackendAPI.Models;
using BackendAPI.Services;
using BackendAPI.Services.IServices;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace BackendAPI.Controllers;
public class AccountController : BaseApiController
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly TokenService _tokenService;
    private readonly DataContext _dataContext;
    private readonly IMemoryCache _memoryCache;
    private readonly SendGridClient _sendGridClient;
    private readonly IAccountServices _accountServices;

    public AccountController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, TokenService tokenService, DataContext dataContext,
         IMemoryCache memoryCache, SendGridClient sendGridClient,IAccountServices accountServices)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _tokenService = tokenService;
        _dataContext = dataContext;
        _memoryCache = memoryCache;
        _sendGridClient = sendGridClient;
        _accountServices = accountServices;
    }


    [HttpGet("ShowUser")]
    public async Task<IActionResult> Get()
    {
        var result = await _userManager.Users.ToListAsync();
        List<Object> users = new();
        foreach (var user in result)
        {
            var userRole = await _userManager.GetRolesAsync(user);
            users.Add(new { user.UserName, userRole });
        }
        return Ok(users);
    }

    [HttpGet("ShowAllUser Service!")]
    public async Task<ActionResult> ShowAllUser()
    {
        return HandleResult(await _accountServices.AllUsers());
    }

    [HttpPost("AddRole Service!")]
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


    [HttpPost("login Service!")]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        var LoginValidator = new LoginDtoValidator();
        var LoginValidationResult = LoginValidator.Validate(dto);

        if (!LoginValidationResult.IsValid)
        {
            var errors = LoginValidationResult.Errors.Select(error => error.ErrorMessage).ToList();
            return BadRequest(new { Message = "Validation Login Error", Errors = errors });
        }

       return HandleResult (await _accountServices.LoginAsync(dto));
    }

    [HttpPost("register Service!")]
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

    [HttpPost("ChangePasswordForLogin Service!")]
    public async Task<ActionResult> ChangePassword(ChangePasswordDto dto)
    {
        var validator = new ChangePasswordValidator();
        var resultvalidate = validator.Validate(dto);

        if (!resultvalidate.IsValid)
        {
            var errors = resultvalidate.Errors.Select(x => x.ErrorMessage).ToList();
            return BadRequest(new { Message = "Validation Change Password is Emtry", Errors = errors });
        }

        return HandleResult (await _accountServices.ChangePasswordAsync(dto));
    }

    [HttpPost("ChangeEmailForLogin Service!")]
    public async Task<ActionResult> ChangeUserEmail(ChangeUserEmailDto dto)
    {
        var validator = new ChangeEmailValidator();
        var resultvalidate = validator.Validate(dto);
        if (!resultvalidate.IsValid)
        {
            var errors = resultvalidate.Errors.Select(x => x.ErrorMessage).ToList();
            return BadRequest(new { Message = "Validation Change Email is Emtry", Errors = errors });
        } 
         
        return HandleResult(await _accountServices.ChangeUserEmailAsync(dto));
    }

    [HttpPost("ChangeUserNameForLogin Service!")]
    public async Task<ActionResult> ChangeUserName(ChangeUserNameDto dto)
    {
        var validator = new ChangeUserNameValidator();
        var resultvalidate = validator.Validate(dto);

        if (!resultvalidate.IsValid)
        {
            var errors = resultvalidate.Errors.Select(x => x.ErrorMessage).ToList();
            return BadRequest(new { Message = "Validation Change User", Errors = errors });
        }
        return HandleResult(await _accountServices.ChangeUserNameAsync(dto));
    }


    [HttpPost("ConfirmEmail Service!")]
    public async Task<ActionResult> ConfirmEmailUser (ConfirmEmailUserDto dto)
    {

        var validator = new ConfirmEmailUserValidator();
        var resultvalidate = validator.Validate(dto);

        if (!resultvalidate.IsValid)
        {
            var errors = resultvalidate.Errors.Select(x => x.ErrorMessage).ToList();
            return BadRequest(new { Message = "Validation ConfirmEmail", Errors = errors });
        }

        return HandleResult(await _accountServices.ConfirmEmailUserAsync(dto));
    }

    [HttpDelete("DeleteUser Service!")]
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

    [HttpPost("ForgetPassword Service!")]
    public async Task<ActionResult> ForgetPassword(ForgetPasswordDto dto)
    {
        var validator = new ForgetPasswordValidator();
        var resultvalidate = validator.Validate(dto);

        if (!resultvalidate.IsValid)
        {
            var errors = resultvalidate.Errors.Select(x => x.ErrorMessage).ToList();
            return BadRequest(new { Message = "Validation ForgetPassword", Errors = errors });
        }

        return HandleResult(await _accountServices.ForgetPasswordAsync(dto));

    }


    [HttpPost("ResendOtpConfirmEmail")]
    public async Task<ActionResult> ResendOtpConfirmEmail(ResendOtpConfirmEmailDto dto)
    {
        var validator = new ResendOtpConfirmEmailValidator();
        var resultvalidate = validator.Validate(dto);

        if (!resultvalidate.IsValid)
        {
            var errors = resultvalidate.Errors.Select(x => x.ErrorMessage).ToList();
            return BadRequest(new { Message = "Validation ForgetPassword", Errors = errors });
        }

        return HandleResult(await _accountServices.ResendOtpConfirmEmailAsync(dto));
    }




}
