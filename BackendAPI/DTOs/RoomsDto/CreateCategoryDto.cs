using BackendAPI.Data;
using FluentValidation;

namespace BackendAPI.DTOs.RoomsDto;
public class CreateCategoryDto
{
    public string Name { get; set; }
    public DateTime DateTimeCreate { get; set; }
    public int Servicefees { get; set; }
    public IFormFile Image { get; set; }
    public string Detail { get; set; }
}

public class CreateCategoryDtoValidator : AbstractValidator<CreateCategoryDto>
{
    public CreateCategoryDtoValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Name is empty");
        RuleFor(x => x.Servicefees).NotEmpty().WithMessage("Servicefees is empty");
    }

}