using BackendAPI.Data;
using FluentValidation;

namespace BackendAPI.DTOs.RoomsDto;
public class CreateCategoryDto
{
    public string Name { get; set; }
    public DateTime DateTimeCreate { get; set; }
    public int Servicefees { get; set; }
    public int? ServicefeesforMember { get; set; }
    public IFormFile Image { get; set; }
    public string Detail { get; set; }
}
