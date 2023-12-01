using BackendAPI.Core;
using BackendAPI.Data;
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

    [HttpPost("CreateLocation Service!")]
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

    [HttpPost("Update Locations Service")]
    public async Task<ActionResult> UpdateLocation([FromForm]UpdateLocationDto dto)
    {

        var validator = new UpdateLocationDtoValidator();
        var valida = validator.Validate(dto);

        if (!valida.IsValid)
        {
            var errors = valida.Errors.Select(error => error.ErrorMessage).ToList();
            return BadRequest(new { Message = "Validation Create Error", Errors = errors });
        }


        return HandleResult(await _locationService.UpdateLocationAsync(dto));
    }

    [HttpGet("ShowLoaction Service!")]
    public async Task<ActionResult> ShowLocation()
    {
        //return HandleResult(await _locationService.ShowLocationAsync());
        var result = await _locationService.ShowLocationAsync();

        var locations = result.Value;

        if(locations != null && locations.Any())
        {
            var response = locations.Select(LocationResponse.FromLocation).ToList();
            return Ok(response);
        }
        else
        {
            return NotFound("No locations found");
        }
    }

    [HttpDelete("DeleteLocation Service!")]                                                                                                                                                                                                                                                                           
    public async Task<ActionResult> DeleteLocation(int id)
    {
       return HandleResult(await _locationService.DeleteLocationAsync(id));
    }




}