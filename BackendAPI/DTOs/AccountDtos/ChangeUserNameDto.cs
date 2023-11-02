using FluentValidation;

namespace BackendAPI.DTOs.AccountDtos;
public class ChangeUserNameDto
{
    public string UserId { get; set; }
    public string UserName { get; set; }
    public string NewUserName { get; set; }
}


public class ChangeUserNameValidator : AbstractValidator<ChangeUserNameDto>
{
    public ChangeUserNameValidator()
    {
        RuleFor(x => x.UserId).NotEmpty().WithMessage("UserId is Emtry");
        RuleFor(x => x.UserName).NotEmpty().WithMessage("UserName is Emtry");
        RuleFor(x => x.NewUserName).NotEmpty().WithMessage("NewUserName is Emtry");
    }
}