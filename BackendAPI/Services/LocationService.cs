using AutoMapper;
using Azure.Core;
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
    private readonly IUploadFileService _uploadFileService;

    public LocationService(DataContext dataContext, IMapper mapper,IUploadFileService uploadFileService)
    {
        _dataContext = dataContext;
        _mapper = mapper;
        _uploadFileService = uploadFileService;
    }
    public async Task<Result<string>> CreateLocationAsync(LocationRequest dto)
    {

        var test = await _dataContext.Locations.FirstOrDefaultAsync(x => x.Name == dto.Name);
        if (test != null)
        {
            return Result<string>.Failure("Room Name is already use.");
        }


        //อัพโหลดไฟล์ Start
        (string errorMessage, List<string> imageNames) = await UploadImageAsync(dto.FormFiles);
        if (!string.IsNullOrEmpty(errorMessage)) return Result<string>.Failure("Fail to UploadImages");


        var map = _mapper.Map<Location>(dto);
        _dataContext.Locations.Add(map);
        await _dataContext.SaveChangesAsync();

        //จัดการไฟล์ในฐานข้อมูล
        if (imageNames.Count > 0)
        {
            var images = new List<LocationImages>();
            foreach (var image in imageNames)
            {
                images.Add(new LocationImages { LocationId = map.Id, Image = image });
            }
            await _dataContext.locationImages.AddRangeAsync(images);
        }


        await _dataContext.SaveChangesAsync();

        var existingCategory = await _dataContext.CategoryLocations.FindAsync(dto.CategoryId);
        if (existingCategory == null)
        {
            return Result<string>.Failure("Category Not Found.");
        }

        string imageFileName = "";
        string uploadDirectory = "wwwroot/LocationImage";

        if (dto.Image != null)
        {
            imageFileName = "Lo_" + Guid.NewGuid() + Path.GetExtension(dto.Image.FileName);
            var imagePath = Path.Combine(uploadDirectory, imageFileName);

            using (var stream = new FileStream(imagePath, FileMode.Create))
            {
                await dto.Image.CopyToAsync(stream);
            }
            map.Image = imageFileName;
            await _dataContext.SaveChangesAsync();
        }
        return Result<string>.Success($"Create Location Success {map.Image}");
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

    public async Task<(string errorMessage, List<string> imageNames)> UploadImageAsync(IFormFileCollection formFiles)
    {
        var errorMessage = string.Empty;
        var imageNames = new List<string>();
        if (_uploadFileService.IsUpload(formFiles))
        {
            errorMessage = _uploadFileService.Validation(formFiles);
            if (string.IsNullOrEmpty(errorMessage))
            {
                //บันทึกลงไฟล์ในโฟลเดอร์ 
                imageNames = await _uploadFileService.UploadImages(formFiles);
            }
        }
        return (errorMessage, imageNames);

    }
}

