using BackendAPI.Core;
using BackendAPI.Data;
using BackendAPI.DTOs;
using BackendAPI.DTOs.AccountDtos;
using BackendAPI.Models;
using BackendAPI.Response;
using BackendAPI.Services;
using BackendAPI.Services.IServices;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using SendGrid;

namespace BackendAPI.Controllers;
public class AccountController : BaseApiController
{
    private readonly DataContext _dataContext;
    private readonly IAccountServices _accountServices;

    public AccountController(DataContext dataContext, IAccountServices accountServices)
    {
        _dataContext = dataContext;
        _accountServices = accountServices;
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

        return HandleResult(await _accountServices.LoginAsync(dto));
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

        return HandleResult(await _accountServices.ChangePasswordAsync(dto));
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
    public async Task<ActionResult> ConfirmEmailUser(ConfirmEmailUserDto dto)
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

    [HttpPost("ResendOtpConfirmEmail Service!")]
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

    [HttpPost("ForgetPassword For Reset Password Service!")]
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

    [HttpPost("ConfirmEmailToForgotPassword Service!")]
    public async Task<ActionResult> ConfirmEmailToForgotPassword(ConfirmForgotPasswordUserDto dto)
    {
        var validator = new ConfirmForgotPasswordUserValidator();
        var resultvalidate = validator.Validate(dto);

        if (!resultvalidate.IsValid)
        {
            var errors = resultvalidate.Errors.Select(x => x.ErrorMessage).ToList();
            return BadRequest(new { Message = "Validation ForgetPassword", Errors = errors });
        }

        return HandleResult(await _accountServices.ConfirmEmailToForgotPasswordAsync(dto));
    }

    [HttpPost("SendOTPToForgotPassword Service!")]
    public async Task<ActionResult> SendOtpToForgotPassword(SendOtpToForgotPasswordDto dto)
    {
        var validator = new SendOtpToForgotPasswordValidator();
        var resultvalidate = validator.Validate(dto);

        if (!resultvalidate.IsValid)
        {
            var errors = resultvalidate.Errors.Select(x => x.ErrorMessage).ToList();
            return BadRequest(new { Message = "Validation DeleteUser", Errors = errors });
        }

        return HandleResult(await _accountServices.sendOtpToForgotPasswordAsync(dto));

    }

    [HttpPost("ResendOTPToForgotPassword Service!")]
    public async Task<ActionResult> ResendOTPToForgotPassword(ResendOtpToForgotPasswordDto dto)
    {
        var validator = new ResendOtpToForgotPasswordValidator();
        var resultvalidate = validator.Validate(dto);

        if (!resultvalidate.IsValid)
        {
            var errors = resultvalidate.Errors.Select(x => x.ErrorMessage).ToList();
            return BadRequest(new { Message = "Validation DeleteUser", Errors = errors });
        }
        return HandleResult(await _accountServices.ResendOtpToForgotPasswordAsync(dto));
    }


}
