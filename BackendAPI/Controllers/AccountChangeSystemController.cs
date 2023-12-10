using BackendAPI.Data;
using BackendAPI.DTOs.AccountDtos;
using BackendAPI.Services.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BackendAPI.Controllers
{
    public class AccountChangeSystemController : BaseApiController
    {
        private readonly IAccountServices _accountServices;

        public AccountChangeSystemController(IAccountServices accountServices)
        {
            _accountServices = accountServices;
        }

        [HttpPost("ChangePasswordForLoginService")]
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

        [HttpPost("ChangeEmailForLoginService")]
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

        [HttpPost("ChangeUserNameForLoginService")]
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

        [HttpPost("ForgetPasswordForResetPasswordService")]
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


    }
}
