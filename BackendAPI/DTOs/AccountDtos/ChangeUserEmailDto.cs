using FluentValidation;

namespace BackendAPI.DTOs.AccountDtos;

    public class ChangeUserEmailDto
    {
        public string UserId { get; set; }
        public string Email { get; set; }
        public string NewEmail { get; set; }
    }


public class ChangeEmailValidator : AbstractValidator<ChangeUserEmailDto>
{
    public ChangeEmailValidator()
    {
        RuleFor(Change => Change.UserId).NotEmpty().WithMessage("UserId is empty");
        RuleFor(Change => Change.Email).NotEmpty().WithMessage("Email is empty");
        RuleFor(Change => Change.NewEmail).NotEmpty().WithMessage("NewEmail is empty");
    }
}

