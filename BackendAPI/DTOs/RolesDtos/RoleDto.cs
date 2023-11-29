using BackendAPI.DTOs;
using BackendAPI.DTOs.RolesDtos;
using FluentValidation;

namespace BackendAPI.DTOs.RolesDtos
{
    public class RoleDto
    {
        public string Name { get; set; }
    }
}


public class RoleValidator : AbstractValidator<RoleDto>
{
    public RoleValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Name is empty");
    }
}