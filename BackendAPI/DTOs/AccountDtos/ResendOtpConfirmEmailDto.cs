using FluentValidation;

namespace BackendAPI.DTOs.AccountDtos;
    public class ResendOtpConfirmEmailDto
    {
    public string Email { get; set; }
    }


public class ResendOtpConfirmEmailValidator : AbstractValidator<ResendOtpConfirmEmailDto>
{
    public ResendOtpConfirmEmailValidator()
    {
        RuleFor(x => x.Email).NotEmpty().WithMessage("Email is empty");
    }
}