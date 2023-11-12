using BackendAPI.Data;
using FluentValidation;

namespace BackendAPI.DTOs.RoomsDto;
    public class CreateCategoryDto
    {
        public string Name { get; set; }
        public DateTime DateTimeCreate { get; set; }
    }

public class CreateCategoryDtoValidator : AbstractValidator<CreateCategoryDto>
{
    public CreateCategoryDtoValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Name is empty");
        RuleFor(x => x.DateTimeCreate).NotEmpty().WithMessage("DateTimeCreate is empty");
    }

}