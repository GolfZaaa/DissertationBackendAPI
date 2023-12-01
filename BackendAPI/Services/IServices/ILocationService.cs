using BackendAPI.Core;
using BackendAPI.DTOs.RoomsDto;
using BackendAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace BackendAPI.Services.IServices;
    public interface ILocationService
    {
    Task<Result<string>> CreateLocationAsync(LocationRequest dto);
    Task<Result<List<Location>>> ShowLocationAsync();
    Task<Result<string>> DeleteLocationAsync(int id);
    Task<Result<string>> UpdateLocationAsync([FromForm] UpdateLocationDto dto);
    Task<(string errorMessage, List<string> imageNames)> UploadImageAsync(IFormFileCollection formFiles);
}
