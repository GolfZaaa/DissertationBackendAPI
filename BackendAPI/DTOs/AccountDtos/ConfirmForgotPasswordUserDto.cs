using FluentValidation;

namespace BackendAPI.DTOs.AccountDtos;
public class ConfirmForgotPasswordUserDto
{
    public string Email { get; set; }
    public string TokenConfirm { get; set; }
}


public class ConfirmForgotPasswordUserValidator : AbstractValidator<ConfirmForgotPasswordUserDto>
{
    public ConfirmForgotPasswordUserValidator()
    {
        RuleFor(x => x.Email).NotEmpty().WithMessage("Email is empty");
        RuleFor(x => x.TokenConfirm).NotEmpty().WithMessage("TokenConfirm is empty");
    }
}