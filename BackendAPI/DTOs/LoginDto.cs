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
        RuleFor(login => login.Username).NotEmpty().WithMessage("Username is empty").Length(5, 20).WithMessage("ชื่อผู้ใช้ต้องมีความยาวระหว่าง 5 ถึง 20 ตัว").Matches("^[a-zA-Z0-9]*$").WithMessage("ชื่อผู้ใช้ต้องประกอบด้วยตัวอักษรและตัวเลขเท่านั้น");
        RuleFor(login => login.Password).NotEmpty().WithMessage("Password is empty");
    }
}
