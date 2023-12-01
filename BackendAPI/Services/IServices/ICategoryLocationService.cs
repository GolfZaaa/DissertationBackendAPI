using BackendAPI.Core;
using BackendAPI.DTOs.CategoryDtos;
using BackendAPI.DTOs.RoomsDto;

namespace BackendAPI.Services.IServices
{
    public interface ICategoryLocationService
    {
        Task<Result<dynamic>> CategoryRoomallAsync();
        Task<Result<string>> CreateCategoryAsync(CreateCategoryDto dto);
        Task<Result<string>> DeleteCategorysAsync(int id);
        Task<Result<string>> UpdateCategoryAsync(UpdateCategoryDto dto);

    }
}
