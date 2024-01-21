using BackendAPI.Core;
using BackendAPI.Data;
using BackendAPI.DTOs.AccountDtos;
using BackendAPI.Models;
using BackendAPI.Response;
using BackendAPI.Services;
using BackendAPI.Services.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BackendAPI.Controllers
{
    public class AccountChangeSystemController : BaseApiController
    {
        private readonly IAccountServices _accountServices;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUploadFileSingleService _uploadFileSingleService;
        private readonly DataContext _dataContext;

        public AccountChangeSystemController(IAccountServices accountServices, UserManager<ApplicationUser> userManager,IUploadFileSingleService uploadFileSingleService,DataContext dataContext)
        {
            _accountServices = accountServices;
            _userManager = userManager;
            _uploadFileSingleService = uploadFileSingleService;
            _dataContext = dataContext;
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
        [HttpPost("UploadProfileImageAsync")]
        public async Task<ActionResult> UploadProfileImageAsync([FromForm]UploadProfileImageDTO dto)
        {
            var user = await _userManager.FindByIdAsync(dto.userId);

            if (user == null)
            {
                return HandleResult(Result<string>.Failure("User Not Found"));
            }


            if (dto.ProfileImage == null || dto.ProfileImage.Length == 0)
            {
                return HandleResult(Result<string>.Failure("No file uploaded."));
            }

            (string errorMessgeMain, string imageNames) = await UploadImageMainAsync(dto.ProfileImage);
            user.ProfileImage = imageNames;
            await _dataContext.SaveChangesAsync();
            return HandleResult(Result<string>.Success("Profile image uploaded successfully."));
        }


        private async Task<(string errorMessge, string imageNames)> UploadImageMainAsync(IFormFile formfile)
        {
            var errorMessge = string.Empty;
            var imageName = string.Empty;

            if (_uploadFileSingleService.IsUpload(formfile))
            {
                errorMessge = _uploadFileSingleService.Validation(formfile);
                if (errorMessge is null)
                {
                    imageName = await _uploadFileSingleService.UploadImagesProfileUser(formfile);
                }
            }

            return (errorMessge, imageName);
        }


    }
}
