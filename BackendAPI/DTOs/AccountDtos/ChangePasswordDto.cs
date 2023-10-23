using FluentValidation;

namespace BackendAPI.DTOs.AccountDtos;

public class ChangePasswordDto
{
    public string UserId { get; set; }
    public string Password { get; set; }
    public string NewPassword { get; set; }
}


public class ChangePasswordValidator : AbstractValidator<ChangePasswordDto>
{
    public ChangePasswordValidator()
    {
        RuleFor(Change => Change.UserId).NotEmpty().WithMessage("UserId is empty");
        RuleFor(Change => Change.Password).NotEmpty().WithMessage("Password is empty");
        RuleFor(Change => Change.NewPassword).NotEmpty().WithMessage("NewPassword is empty");
    }
}