using BackendAPI.DTOs.RolesDtos;
using FluentValidation;

namespace BackendAPI.DTOs.RolesDtos;
    public class RoleUpdateDto : RoleDto
    {
        public string UpdateName { get; set; }
    }


public class RoleUpdateValidator : AbstractValidator<RoleUpdateDto>
{
    public RoleUpdateValidator()
    {
        RuleFor(x => x.UpdateName).NotEmpty().WithMessage("UpdateName is empty");
    }
}