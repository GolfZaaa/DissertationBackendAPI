using BackendAPI.Controllers;
using BackendAPI.Core;
using BackendAPI.DTOs.AccountDtos;
using BackendAPI.Models;
using Microsoft.AspNetCore.Identity;

namespace BackendAPI.Services
{
    public class AccountService : BaseApiController, IAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public AccountService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }


        public async Task<object> ChangeUserEmailAsync(ChangeUserEmailDto dto)
        {
            var validator = new ChangeEmailValidator();
            var resultvalidate = validator.Validate(dto);
            if (!resultvalidate.IsValid)
            {
                var errors = resultvalidate.Errors.Select(x => x.ErrorMessage).ToList();
                return BadRequest(new { Message = "Validation Change Email is Emtry", Errors = errors });
            }
            var user = await _userManager.FindByIdAsync(dto.UserId);

            if (user == null)
            {
                return HandleResult(Result<string>.Failure("User Not Found."));
            }
            if (user.Email == dto.NewEmail)
            {
                return HandleResult(Result<string>.Failure("The new Email is the same as the current Email you are using. Please enter a Email that is different from the current one."));
            }
            var result = await _userManager.SetEmailAsync(user, dto.NewEmail);
            user.EmailConfirmed = true;
            await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                return HandleResult(Result<string>.Failure("Fail to SetEmail."));
            }
            return HandleResult(Result<string>.Success("Change Email Successfuly."));
        }
    }
}
