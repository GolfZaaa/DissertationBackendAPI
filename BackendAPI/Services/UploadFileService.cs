using BackendAPI.Services.IServices;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace BackendAPI.Services;
public class UploadFileService : IUploadFileService
{
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly IConfiguration _configuration;

    public UploadFileService(IWebHostEnvironment webHostEnvironment, IConfiguration configuration)
    {
        _webHostEnvironment = webHostEnvironment;     //เข้าหา wwwroot
        _configuration = configuration;     //เข้าหา appsettings.json  
    }

    public Task DeleteFileImages(List<string> files)
    {
        string wwwRootPath = _webHostEnvironment.WebRootPath;
        foreach (var item in files)
        {
            var file = Path.Combine("LocationImages", item);
            var oldImagePath = Path.Combine(wwwRootPath, file);
            if (System.IO.File.Exists(oldImagePath))
            {
                System.IO.File.Delete(oldImagePath);
            }
        }
        return Task.CompletedTask;
    }

    public bool IsUpload(IFormFileCollection formFiles)
    {
        return formFiles != null && formFiles?.Count > 0;
    }

    public async Task<List<string>> UploadImages(IFormFileCollection formFiles)
    {
        var listFileName = new List<string>();

        //จัดการเส้นทาง
        string wwwRootPath = _webHostEnvironment.WebRootPath;
        var uploadPath = Path.Combine(wwwRootPath, "LocationImages");
        if (!Directory.Exists(uploadPath)) Directory.CreateDirectory(uploadPath);
        foreach (var formFile in formFiles)
        {
            string fileName = "LoImages_"+Guid.NewGuid().ToString() + Path.GetExtension(formFile.FileName);
            string fullName = Path.Combine(uploadPath, fileName);
            using (var stream = File.Create(fullName))
            {
                await formFile.CopyToAsync(stream);
            }
            listFileName.Add(fileName);
        }
        return listFileName;
    }

    public string Validation(IFormFileCollection formFiles)
    {
        foreach (var file in formFiles)
        {
            if (!ValidationExtension(file.FileName))
                return "Invalid File Extension";

            if (!ValidationSize(file.Length))
                return "The file is too large";
        }
        return null;
    }

    public bool ValidationExtension(string filename)
    {
        string[] permittedExtensions = { ".jpg", ".png" , ".jpeg" };
        string extension = Path.GetExtension(filename).ToLowerInvariant();


        if (string.IsNullOrEmpty(extension) || !permittedExtensions.Contains(extension))
            return false;

        return true;
    }
    public bool ValidationSize(long fileSize) => _configuration.GetValue<long>("FileSizeLimit") > fileSize;
}
