using BackendAPI.DTOs.RoomsDto;
using FluentValidation;

namespace BackendAPI.DTOs.CategoryDtos;
    public class UpdateCategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime DateTimeCreate { get; set; }
        public int Servicefees { get; set; }

    }


public class UpdateCategoryDtoValidator : AbstractValidator<UpdateCategoryDto>
{
    public UpdateCategoryDtoValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Name is empty");
        RuleFor(x => x.DateTimeCreate).NotEmpty().WithMessage("DateTimeCreate is empty");
        RuleFor(x => x.Servicefees).NotEmpty().WithMessage("Servicefees is empty");
    }

}