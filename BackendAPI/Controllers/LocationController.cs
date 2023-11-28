using BackendAPI.Core;
using BackendAPI.Data;
using BackendAPI.DTOs.RoomsDto;
using BackendAPI.Models;
using BackendAPI.Response;
using BackendAPI.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackendAPI.Controllers;

public class LocationController : BaseApiController
{
    private readonly DataContext _dataContext;
    private readonly ILocationService _locationService;

    public LocationController(DataContext dataContext,ILocationService locationService)
    {
        _dataContext = dataContext;
        _locationService = locationService;
    }


    [HttpPost("CreateCategory Service!")]
    public async Task<IActionResult> CreateCategory(CreateCategoryDto dto)
    {
        var Create = new CreateCategoryDtoValidator();
        var result = Create.Validate(dto);

        if (!result.IsValid)
        {
            var errors = result.Errors.Select(error => error.ErrorMessage).ToList(); 
            return BadRequest(new { Message = "Validation Create Error", Errors = errors });
        }

        return HandleResult(await _locationService.CreateCategoryAsync(dto));
    }

    [HttpGet("ShowCategoryAll Service!")]
    public async Task<IActionResult> ShowCategory()
    {
        return HandleResult(await _locationService.CategoryRoomallAsync());
    }

    [HttpDelete("DeleteCategorys Service!")]
    public async Task<ActionResult> DeleteCategorys(int id)
    {
        return HandleResult(await _locationService.DeleteCategorysAsync(id));
    }

    [HttpPost("CreateLocation Service!")]
    public async Task<IActionResult> CreateLocation([FromForm] CreateLocationDto dto)
    {
        var validator = new CreateLocationDtoValidator();
        var result = validator.Validate(dto);

        if (!result.IsValid)
        {
            var errors = result.Errors.Select(error => error.ErrorMessage).ToList();
            return BadRequest(new { Message = "Validation Create Error", Errors = errors });
        }

        return HandleResult(await _locationService.CreateLocationAsync(dto));
    }

    [HttpGet("ShowLoaction Service!")]
    public async Task<ActionResult> ShowLocation()
    {
        return HandleResult(await _locationService.ShowLocationAsync());
    }

    [HttpDelete("DeleteLocation Service!")]                                                                                                                                                                                                                                                                           
    public async Task<ActionResult> DeleteLocation(int id)
    {
       return HandleResult(await _locationService.DeleteLocationAsync(id));
    }

}