using BackendAPI.DTOs.AccountDtos;
using FluentValidation;

namespace BackendAPI.DTOs.AccountDtos;
    public class ResendOtpToForgotPasswordDto
    {
        public string Email { get; set; }
    }


public class ResendOtpToForgotPasswordValidator : AbstractValidator<ResendOtpToForgotPasswordDto>
{
    public ResendOtpToForgotPasswordValidator()
    {
        RuleFor(x => x.Email).NotEmpty().WithMessage("Email is empty");
    }
}