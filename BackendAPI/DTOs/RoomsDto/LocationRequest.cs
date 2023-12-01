using FluentValidation;

namespace BackendAPI.DTOs.RoomsDto;
    public class LocationRequest
    {
    public int? Id { get; set; }
    public string Name { get; set; }
    public int Capacity { get; set; }
    public IFormFile Image { get; set; }
    public string PlaceDescription { get; set; }
    public int CategoryId { get; set; }
    public IFormFileCollection? FormFiles { get; set; }
}


public class LocationRequestValidator : AbstractValidator<LocationRequest>
{
    public LocationRequestValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("LocationName is empty");
        RuleFor(x => x.Capacity).NotEmpty().WithMessage("Capacity is empty");
        RuleFor(x => x.Image).NotEmpty().WithMessage("Image is empty");
        RuleFor(x => x.PlaceDescription).NotEmpty().WithMessage("PlaceDescription is empty");
        RuleFor(x => x.CategoryId).NotEmpty().WithMessage("CategoryId is empty");
    }

}