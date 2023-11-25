using BackendAPI.Data;
using BackendAPI.Models;
using FluentValidation;

namespace BackendAPI.DTOs.RoomsDto;

public class CreateLocationDto
{
    public string LocationName { get; set; }
    public int Capacity { get; set; }
    public IFormFile Image { get; set; }
    public string PlaceDescription { get; set; }
    public int CategoryId { get; set; }
}


public class CreateLocationDtoValidator : AbstractValidator<CreateLocationDto>
{
    public CreateLocationDtoValidator()
    {
        RuleFor(x => x.LocationName).NotEmpty().WithMessage("LocationName is empty");
        RuleFor(x => x.Capacity).NotEmpty().WithMessage("Capacity is empty");
        RuleFor(x => x.PlaceDescription).NotEmpty().WithMessage("PlaceDescription is empty");
        RuleFor(x => x.CategoryId).NotEmpty().WithMessage("CategoryId is empty");
    }

}