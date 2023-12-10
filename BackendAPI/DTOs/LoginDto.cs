using FluentValidation;

namespace BackendAPI.DTOs;
public class LoginDto
{
    public string Username { get; set; }
    public string Password { get; set; }
}
public class LoginDtoValidator : AbstractValidator<LoginDto>
{
    public LoginDtoValidator()
    {
        RuleFor(login => login.Username).NotEmpty().WithMessage("Username is empty");
        RuleFor(login => login.Password).NotEmpty().WithMessage("Password is empty");
    }
}
