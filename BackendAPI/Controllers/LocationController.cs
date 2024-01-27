using BackendAPI.Core;
using BackendAPI.Data;
using BackendAPI.DTOs;
using BackendAPI.DTOs.RoomsDto;
using BackendAPI.Models;
using BackendAPI.Response;
using BackendAPI.Services.IServices;
using FluentValidation;
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

    [HttpPost("CreateLocationService")]
    public async Task<IActionResult> CreateLocation([FromForm] LocationRequest dto)
    {
        var validator = new LocationRequestValidator();
        var result = validator.Validate(dto);

        if (!result.IsValid)
        {
            var errors = result.Errors.Select(error => error.ErrorMessage).ToList();
            return BadRequest(new { Message = "Validation Create Error", Errors = errors });
        }

        return HandleResult(await _locationService.CreateLocationAsync(dto));
    }

    [HttpPost("Update LocationsService")]
    public async Task<ActionResult> UpdateLocation([FromForm] LocationRequest dto)
    {
        return HandleResult(await _locationService.UpdateLocationAsync(dto));
    }

    [HttpGet("GetLocationByIdSerivce")]
    public async Task<IActionResult> GetByIdAsync(int id)
    {
        var locationResult = await _locationService.GetByIdAsync(id);
        var locationResponse = LocationResponse.FromLocation(locationResult.Value);
        return Ok(locationResponse);
    }

    [HttpGet("ShowLoactionService")]
    public async Task<ActionResult> ShowLocation()
    {
        return HandleResult(await _locationService.ShowLocationAsync());
    }

    [HttpDelete("DeleteLocationService")]                                                                                                                                                                                                                                                                           
    public async Task<ActionResult> DeleteLocation(int id)
    {
       return HandleResult(await _locationService.DeleteLocationAsync(id));
    }

    [HttpPost("TurnOnOffLocation")]
    public async Task<ActionResult> TurnOnOffLocation(TurnOnOffDto dto)
    {
        var location = await _dataContext.Locations.FirstOrDefaultAsync(x => x.Id == dto.Id);
        if (location == null)
        {
            HandleResult(Result<string>.Failure("Not Found Location"));
        }

        if (dto.StatusOnOff == 0)
        {
            location.StatusOnOff = 0;
        }
        else
        {
            location.StatusOnOff = 1;
        }
        await _dataContext.SaveChangesAsync();
        return Ok(location);
    }

}