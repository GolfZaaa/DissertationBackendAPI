using AutoMapper;
using BackendAPI.Core;
using BackendAPI.Data;
using BackendAPI.DTOs.RoomsDto;
using BackendAPI.Models;
using BackendAPI.Response;
using BackendAPI.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackendAPI.Services;
public class LocationService : ILocationService
{
    private readonly DataContext _dataContext;
    private readonly IMapper _mapper;

    public LocationService(DataContext dataContext, IMapper mapper)
    {
        _dataContext = dataContext;
        _mapper = mapper;
    }

    public async Task<Result<dynamic>> CategoryRoomallAsync()
    {
        var result = await _dataContext.CategoryRooms.ToListAsync();
        if (result == null || result.Count == 0)
        {
            return Result<dynamic>.Failure("Notfound Category");
        }
        return Result<dynamic>.Success(result);
    }

    public async Task<Result<string>> CreateCategoryAsync(CreateCategoryDto dto)
    {
        if (_dataContext.CategoryRooms.Any(x => x.Name == dto.Name))
        {
            return Result<string>.Failure("Category Name is already in use.");
        }

        var category = new CategoryRoom
        {
            Name = dto.Name,
            DateTimeCreate = DateTime.Now,
            Servicefees = dto.Servicefees,
        };
        _dataContext.Add(category);
        await _dataContext.SaveChangesAsync();

        return Result<string>.Success("Category created successfully");
    }

    public async Task<Result<string>> CreateLocationAsync(CreateLocationDto dto)
    {
        var existingCategory = await _dataContext.CategoryRooms.FindAsync(dto.CategoryId);
        if (existingCategory == null)
        {
            return Result<string>.Failure("Category Not Found.");
        }

        var map = _mapper.Map<Location>(dto);

        if (_dataContext.Locations.Any(x => x.Name == dto.LocationName))
        {
            return Result<string>.Failure("Room Name is already in use.");
        }

        string imageFileName = "";
        string uploadDirectory = "wwwroot/LocationImage";

        if (dto.Image != null)
        {
            imageFileName = "Location_" + Guid.NewGuid() + Path.GetExtension(dto.Image.FileName);
            var imagePath = Path.Combine(uploadDirectory, imageFileName);

            using (var stream = new FileStream(imagePath, FileMode.Create))
            {
                await dto.Image.CopyToAsync(stream);
            }
        }

        var room = new Location
        {
            Name = dto.LocationName,
            Capacity = dto.Capacity,
            Image = imageFileName,
            PlaceDescription = dto.PlaceDescription,
            Status = 1,
            CategoryId = dto.CategoryId,
        };

        _dataContext.Locations.Add(room);
        await _dataContext.SaveChangesAsync();
        return Result<string>.Success("Location created successfully");
    }

    public async Task<Result<string>> DeleteCategorysAsync(int id)
    {
        var category = await _dataContext.CategoryRooms.FindAsync(id);

        if (category == null)
        {
            return Result<string>.Failure("Not Found Category");
        }

        _dataContext.CategoryRooms.Remove(category);
        await _dataContext.SaveChangesAsync();
        return Result<string>.Success($"Delete Category ID{id} Success");
    }

    public async Task<Result<string>> DeleteLocationAsync(int id)
    {
        var location = await _dataContext.Locations.FindAsync(id);

        if (location == null)
        {
            return Result<string>.Failure("Not Found Location");
        }
        _dataContext.Locations.Remove(location);

        if(!string.IsNullOrEmpty(location.Image))
        {
            var imagePath = Path.Combine("wwwroot/LocationImage", location.Image);
            if(File.Exists(imagePath))
            {
                File.Delete(imagePath);
            }
        }
         _dataContext.Locations.Remove(location);
        await _dataContext.SaveChangesAsync();
        return Result<string>.Success($"Delete Location ID {id} Success");
    }
    public async Task<Result<List<Location>>> ShowLocationAsync()
    {
        var result = await _dataContext.Locations.Include(a=>a.locationImages).OrderByDescending(x=>x.Id).ToListAsync();
        if (result == null || result.Count == 0)
        {
            return Result< List<Location>>.Failure("Notfound Location");
        }
        return Result< List<Location>>.Success(result);
    }

    public async Task<Result<string>> UpdateLocationAsync([FromForm] UpdateLocationDto dto)
    {
        var result = await _dataContext.Locations.FindAsync(dto.Id);

        if (result == null) return Result<string>.Failure("Not Found Location");

        string oldImageName = result.Image;

        result.Name = dto.LocationName;
        result.Capacity = dto.Capacity;
        result.PlaceDescription = dto.PlaceDescription;
        result.CategoryId = dto.CategoryId;

        string PathImage = "wwwroot/LocationImage";
        if (dto.Image != null)
        {
            string ImageFileName = "Location_" + Guid.NewGuid() + Path.GetExtension(dto.Image.FileName);

            var imagePath = Path.Combine(PathImage, ImageFileName);

            using (var stream = new FileStream(imagePath, FileMode.Create))
            {
                await dto.Image.CopyToAsync(stream);
            }

            if (!string.IsNullOrEmpty(oldImageName))
            {
                string oldImageFilePath = Path.Combine(PathImage, oldImageName);
                if (System.IO.File.Exists(oldImageFilePath))
                {
                    System.IO.File.Delete(oldImageFilePath);
                }
                result.Image = ImageFileName;
            }
        }
        await _dataContext.SaveChangesAsync();
        return Result<string>.Success("Update Location Success");
    }
}

