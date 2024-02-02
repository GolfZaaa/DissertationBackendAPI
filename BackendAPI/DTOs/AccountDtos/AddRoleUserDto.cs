using FluentValidation;

namespace BackendAPI.DTOs.AccountDtos;
    public class AddRoleUserDto
    {
    public string UserId { get; set;}
    public string RoleId { get; set;}
    }

public class AddRoleValidator : AbstractValidator<AddRoleUserDto>
{
    public AddRoleValidator()
    {
        RuleFor(Add => Add.UserId).NotEmpty().WithMessage("UserId is empty");
    }
}