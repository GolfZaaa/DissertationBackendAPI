using FluentValidation;

namespace BackendAPI.DTOs.AccountDtos;
    public class SendOtpToForgotPasswordDto
    {
        public string Email { get; set; }

    }

public class SendOtpToForgotPasswordValidator : AbstractValidator<SendOtpToForgotPasswordDto>
{
    public SendOtpToForgotPasswordValidator()
    {
        RuleFor(x => x.Email).NotEmpty().WithMessage("Email is empty");
    }
}