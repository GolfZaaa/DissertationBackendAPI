using BackendAPI.Core;
using BackendAPI.DTOs.RoomsDto;

namespace BackendAPI.Services.IServices;
    public interface ILocationService
    {
    Task<Result<string>> CreateCategoryAsync(CreateCategoryDto dto);
    Task<Result<dynamic>> CategoryRoomallAsync();
    Task<Result<string>> DeleteCategorysAsync(int id);
    Task<Result<string>> CreateLocationAsync(CreateLocationDto dto);
    Task<Result<dynamic>> ShowLocationAsync();
    Task<Result<string>> DeleteLocationAsync(int id);
}
