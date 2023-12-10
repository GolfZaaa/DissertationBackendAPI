using BackendAPI.DTOs.AccountDtos;
using BackendAPI.Services.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BackendAPI.Controllers
{
    public class AccountOTPController : BaseApiController
    {
        private readonly IAccountServices _accountServices;

        public AccountOTPController(IAccountServices accountServices)
        {
            _accountServices = accountServices;
        }


        [HttpPost("ConfirmEmailService")]
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

        [HttpPost("ResendOtpConfirmEmailService")]
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

        [HttpPost("ConfirmEmailToForgotPasswordService")]
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

        [HttpPost("SendOTPToForgotPasswordService")]
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

        [HttpPost("ResendOTPToForgotPasswordService")]
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
}
