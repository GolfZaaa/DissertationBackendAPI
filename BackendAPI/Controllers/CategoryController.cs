using BackendAPI.Core;
using BackendAPI.Data;
using BackendAPI.DTOs.RoomsDto;
using BackendAPI.Models;
using BackendAPI.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackendAPI.Controllers;

public class CategoryController : BaseApiController
{
    private readonly DataContext _dataContext;

    public CategoryController(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    [HttpPost("CreateCategory")]
    public async Task<IActionResult> CreateCategory(CreateCategoryDto dto)
    {
        var Create = new CreateCategoryDtoValidator();
        var result = Create.Validate(dto);

        if (!result.IsValid)
        {
            var errors = result.Errors.Select(error => error.ErrorMessage).ToList(); 
            return BadRequest(new { Message = "Validation Create Error", Errors = errors });
        }
        if (_dataContext.CategoryRooms.Any(x => x.Name == dto.Name))
        {
            return HandleResult(Result<string>.Failure("Category Name is already in use."));
        }
        var category = new CategoryRoom
        {
            Name = dto.Name,
            DateTimeCreate = DateTime.Now,
        };
        _dataContext.Add(category);
        await _dataContext.SaveChangesAsync();

        return HandleResult(Result<string>.Success("Category created successfully")); 
    }

    [HttpGet("ShowCategoryRoomAll")]
    public async Task<IActionResult> CategoryRoomall()
    {
        var result = await _dataContext.CategoryRooms.ToListAsync();
        if(result == null || result.Count == 0)
        {
            return HandleResult(Result<string>.Failure("Notfound Category"));
        }
        return Ok(result);
    }


    [HttpPost("CreateRooms")]
    public async Task<IActionResult> CreateRooms([FromForm] RoomsDto dto)
    {
        var validator = new RoomsDtoValidator();
        var result = validator.Validate(dto);

        if (!result.IsValid)
        {
            var errors = result.Errors.Select(error => error.ErrorMessage).ToList();
            return BadRequest(new { Message = "Validation Create Error", Errors = errors });
        }

        var existingCategory = await _dataContext.CategoryRooms.FindAsync(dto.CategoryId);
        if (existingCategory == null)
        {
            return NotFound(new ResponseReport { Status = StatusCodes.Status404NotFound, Message = "Category Not Found" });
        }

        if (_dataContext.Rooms.Any(x => x.RoomsName == dto.RoomsName))
        {
            return HandleResult(Result<string>.Failure("Room Name is already in use."));  
        }

        string imageFileName = null;
        string uploadDirectory = "wwwroot/RoomsImage";

        if (dto.Image != null)
        {
            imageFileName = "room_" + Guid.NewGuid() + Path.GetExtension(dto.Image.FileName);
            var imagePath = Path.Combine(uploadDirectory, imageFileName);
        }

        var room = new Room
        {
            RoomsName = dto.RoomsName,
            Capacity = dto.Capacity,
            Image = imageFileName,
            StatusRooms = dto.StatusRooms,
            CategoryId = dto.CategoryId,
        };

        _dataContext.Rooms.Add(room);
        await _dataContext.SaveChangesAsync();


        return HandleResult(Result<string>.Success("Room created successfully"));
    }



}