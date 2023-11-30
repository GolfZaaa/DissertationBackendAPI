using BackendAPI.Core;
using BackendAPI.DTOs.RoomsDto;
using BackendAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace BackendAPI.Services.IServices;
    public interface ILocationService
    {
    Task<Result<string>> CreateCategoryAsync(CreateCategoryDto dto);
    Task<Result<dynamic>> CategoryRoomallAsync();
    Task<Result<string>> DeleteCategorysAsync(int id);
    Task<Result<string>> CreateLocationAsync(CreateLocationDto dto);
    Task<Result<List<Location>>> ShowLocationAsync();
    Task<Result<string>> DeleteLocationAsync(int id);
    Task<Result<string>> UpdateLocationAsync([FromForm] UpdateLocationDto dto);
}
