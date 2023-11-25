using FluentValidation;

namespace BackendAPI.DTOs.AccountDtos;
    public class ForgetPasswordDto
    {
    public string UserId { get; set; }
    public string Password { get; set; }
    public string ConfirmPassword { get; set; }
    }

public class ForgetPasswordValidator : AbstractValidator<ForgetPasswordDto>
{
    public ForgetPasswordValidator()
    {
        RuleFor(x => x.UserId).NotEmpty().WithMessage("UserId is empty");
        RuleFor(x => x.Password)
                   .NotEmpty().WithMessage("Password is empty")
                   .Matches(@"^(?=.*[A-Za-z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$")
                   .WithMessage("Password must contain at least one letter, one digit, one special character, and be at least 8 characters long");
        RuleFor(x => x.ConfirmPassword).NotEmpty().WithMessage("ConfirmPassword is empty");
    }
}