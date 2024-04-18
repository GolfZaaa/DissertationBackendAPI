using BackendAPI.Models;
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
