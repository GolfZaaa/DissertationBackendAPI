using AutoMapper;
using Azure.Core;
using BackendAPI.Core;
using BackendAPI.Data;
using BackendAPI.DTOs.RoomsDto;
using BackendAPI.Models;
using BackendAPI.Response;
using BackendAPI.Services.IServices;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static System.Net.Mime.MediaTypeNames;

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
            return Result<string>.Failure("Location Name is already use.");
        }

        var existingCategory = await _dataContext.CategoryLocations.FindAsync(dto.CategoryId);
        if (existingCategory == null)
        {
            return Result<string>.Failure("Category Not Found.");
        }


        //อัพโหลดไฟล์ Start
        (string errorMessage, List<string> imageNames) = await UploadImageAsync(dto.FormFiles);
        if (!string.IsNullOrEmpty(errorMessage)) return Result<string>.Failure("Fail to UploadImages");


        var map = _mapper.Map<Location>(dto); 

        //จัดการไฟล์ในฐานข้อมูล
        if (imageNames.Count > 0)
        { 
            foreach (var image in imageNames)
            {
                map.locationImages.Add(new LocationImages { Image = image });
            }
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

        map.Category = existingCategory;
        _dataContext.Locations.Add(map);
        await _dataContext.SaveChangesAsync();

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
    public async Task<Result<Location>> GetByIdAsync(int id)
    {
        var result = await _dataContext.Locations.Include(p => p.locationImages).Include(p => p.Category)
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == id);
        return Result<Location>.Success(result);
    }

    public async Task<Result<List<Location>>> ShowLocationAsync()
    {
        var result = await _dataContext.Locations.Include(x=>x.Category).Include(a=>a.locationImages).OrderByDescending(x=>x.Id).ToListAsync();
        if (result == null || result.Count == 0)
        {
            return Result<List<Location>>.Failure("Notfound Location");
        }
        return Result<List<Location>>.Success(result);
    }

    public async Task<Result<string>> UpdateLocationAsync([FromForm] LocationRequest dto)
    {
        var findLocation = await _dataContext.Locations.FindAsync(dto.Id);
        if (findLocation == null)
        {
            return Result<string>.Failure("Location not found");
        }


        var existingCategory = await _dataContext.CategoryLocations.FindAsync(dto.CategoryId);
        if (existingCategory == null)
        {
            return Result<string>.Failure("Category Not Found.");
        }

        if (_dataContext.Locations.Any(x => x.Name == dto.Name))
        {
            return Result<string>.Failure("Location Name is already in use.");
        }

        // ตรวจสอบและอัพโหลดไฟล์
        (string errorMessage, List<string> imageNames) = await UploadImageAsync(dto.FormFiles);
        if (!string.IsNullOrEmpty(errorMessage))
        {
            return Result<string>.Failure("Fail to UploadImages");
        }



        //map.Image = findLocation.Image;

        //var OldImage = _dataContext.Locations.Where(a => a.Id == dto.Id).Select(x => x.Image).FirstOrDefaultAsync();
        string imageFileName = "";
        string uploadDirectory = "wwwroot/LocationImage";
        if (dto.Image != null)
        {
            string filePathToDelete = Path.Combine(uploadDirectory, findLocation.Image);
            if (System.IO.File.Exists(filePathToDelete))
            {
                System.IO.File.Delete(filePathToDelete);
            }

            imageFileName = "Lo_" + Guid.NewGuid() + Path.GetExtension(dto.Image.FileName);
            var imagePath = Path.Combine(uploadDirectory, imageFileName);

            var map = _mapper.Map(dto, findLocation); // ใช้ Mapper เพื่อแม็ปข้อมูลใหม่ลงใน findLocation

            using (var stream = new FileStream(imagePath, FileMode.Create))
            {
                await dto.Image.CopyToAsync(stream);
            }
            map.Image = imageFileName;
            map.Category = existingCategory;

        }
        if (imageNames.Count > 0)
        {
            var newImages = imageNames.Select(image => new LocationImages { Image = image }).ToList();

            findLocation.locationImages.AddRange(newImages);
        }
        //findLocation.Image = imageNames;
        await _dataContext.SaveChangesAsync();

        return Result<string>.Success($"Update Location Success: {findLocation.Image}");
    }

    //[HttpPost("TestDeleteFile")]
    //public async Task<ActionResult> TestDelete(string fileName)
    //{
    //    try
    //    {
    //        string uploadDirectory = "wwwroot/LocationImage"; // ตั้งค่าโฟลเดอร์ที่บันทึกไฟล์

    //        // สร้างที่อยู่ของไฟล์ที่ต้องการลบ
    //        string filePathToDelete = Path.Combine(uploadDirectory, fileName);

    //        if (System.IO.File.Exists(filePathToDelete))
    //        {
    //            // ลบไฟล์ออกจากระบบ
    //            System.IO.File.Delete(filePathToDelete);
    //            return Ok("File deleted successfully.");
    //        }
    //        else
    //        {
    //            return NotFound("File not found.");
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        return StatusCode(500, $"An error occurred: {ex.Message}");
    //    }
    //}


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

