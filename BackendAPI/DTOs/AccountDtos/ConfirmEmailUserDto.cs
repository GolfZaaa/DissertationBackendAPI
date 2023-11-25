using FluentValidation;

namespace BackendAPI.DTOs.AccountDtos;
    public class ConfirmEmailUserDto
    {
        public string TokenConfirm { get; set; }
        public string Email { get; set; }
    }

public class ConfirmEmailUserValidator : AbstractValidator<ConfirmEmailUserDto>
{
    public ConfirmEmailUserValidator()
    {
        RuleFor(x => x.TokenConfirm).NotEmpty().WithMessage("TokenConfirm is Emtry");
        RuleFor(x => x.Email).NotEmpty().WithMessage("Email is Emtry");
    }
}