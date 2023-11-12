using BackendAPI.Data;
using BackendAPI.Models;
using FluentValidation;

namespace BackendAPI.DTOs.RoomsDto;

    public class RoomsDto
    {
        public string RoomsName { get; set; }
        public int Capacity { get; set; }
        public IFormFile Image { get; set; }
        public int StatusRooms { get; set; }
        public int CategoryId { get; set; }
    }


public class RoomsDtoValidator : AbstractValidator<RoomsDto>
{
    public RoomsDtoValidator()
    {
        RuleFor(x => x.RoomsName).NotEmpty().WithMessage("RoomsName is empty");
        RuleFor(x => x.Capacity).NotEmpty().WithMessage("Capacity is empty");
        RuleFor(x => x.StatusRooms).NotEmpty().WithMessage("StatusRooms is empty");
        RuleFor(x => x.CategoryId).NotEmpty().WithMessage("CategoryId is empty");
    }

}