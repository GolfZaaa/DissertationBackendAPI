using BackendAPI.Core;
using BackendAPI.Data;
using BackendAPI.DTOs.CategoryDtos;
using BackendAPI.DTOs.RoomsDto;
using BackendAPI.Models;
using BackendAPI.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackendAPI.Services
{
    public class CategoryLocationService : ICategoryLocationService
    {
        private readonly DataContext _dataContext;

        public CategoryLocationService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<Result<dynamic>> CategoryRoomallAsync()
        {
            var result = await _dataContext.CategoryLocations.ToListAsync();
            if (result == null || result.Count == 0)
            {
                return Result<dynamic>.Failure("Notfound Category");
            }
            return Result<dynamic>.Success(result);
        }

        public async Task<Result<string>> CreateCategoryAsync(CreateCategoryDto dto)
        {
            if (_dataContext.CategoryLocations.Any(x => x.Name == dto.Name))
            {
                return Result<string>.Failure("Category Name is already in use.");
            }

            string imageFileName = "";
            string uploadDirectory = "wwwroot/CategoryLocation";

            if (dto.Image != null)
            {
                imageFileName = "Lo_" + Guid.NewGuid() + Path.GetExtension(dto.Image.FileName);
                var imagePath = Path.Combine(uploadDirectory, imageFileName);

                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    await dto.Image.CopyToAsync(stream);
                }
                await _dataContext.SaveChangesAsync();
            }

            var category = new CategoryLocations
            {
                Name = dto.Name,
                DateTimeCreate = DateTime.Now,
                Servicefees = dto.Servicefees,
                Image = imageFileName,
                Detail = dto.Detail
            };
            _dataContext.Add(category);
            await _dataContext.SaveChangesAsync();

            return Result<string>.Success("Category created successfully");
        }

        public async Task<Result<string>> DeleteCategorysAsync(int id)
        {
            var category = await _dataContext.CategoryLocations.FindAsync(id);

            if (category == null)
            {
                return Result<string>.Failure("Not Found Category");
            }

            _dataContext.CategoryLocations.Remove(category);
            await _dataContext.SaveChangesAsync();
            return Result<string>.Success($"Delete Category ID{id} Success");
        }


        public async Task<Result<string>> UpdateCategoryAsync(UpdateCategoryDto dto)
        {
            var find = await _dataContext.CategoryLocations.FindAsync(dto.Id);

            if (find == null) return Result<string>.Failure("Not Found CategoryID ");

            if (_dataContext.CategoryLocations.Any(x => x.Name == dto.Name))
                return Result<string>.Failure("Category name have already");

            find.Name = dto.Name;
            find.Servicefees = dto.Servicefees;
            find.DateTimeCreate = dto.DateTimeCreate;

            await _dataContext.SaveChangesAsync();

            return Result<string>.Success("Update Category Success");
        }
    }
}
