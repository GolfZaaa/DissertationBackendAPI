using BackendAPI.Core;
using BackendAPI.Data;
using BackendAPI.DTOs.CategoryDtos;
using BackendAPI.DTOs.RoomsDto;
using BackendAPI.Services;
using BackendAPI.Services.IServices;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackendAPI.Controllers
{

    public class CategoryLocationController : BaseApiController
    {
        private readonly DataContext _dataContext;
        private readonly ICategoryLocationService _categoryLocationService;

        public CategoryLocationController(DataContext dataContext,ICategoryLocationService categoryLocationService)
        {
            _dataContext = dataContext;
            _categoryLocationService = categoryLocationService;
        }

        [HttpGet("ShowCategoryAllService")]
        public async Task<IActionResult> ShowCategory()
        {
            return HandleResult(await _categoryLocationService.CategoryRoomallAsync());
        }

        [HttpPost("CreateCategoryService")]
        public async Task<IActionResult> CreateCategory([FromForm]CreateCategoryDto dto)
        {
            var Create = new CreateCategoryDtoValidator();
            var result = Create.Validate(dto);

            if (!result.IsValid)
            {
                var errors = result.Errors.Select(error => error.ErrorMessage).ToList();
                return BadRequest(new { Message = "Validation Create Error", Errors = errors });
            }

            return HandleResult(await _categoryLocationService.CreateCategoryAsync(dto));
        }

        [HttpDelete("DeleteCategorysService")]
        public async Task<ActionResult> DeleteCategorys(int id)
        {
            return HandleResult(await _categoryLocationService.DeleteCategorysAsync(id));
        }

        [HttpGet("GetCategoryUsingLocation")]
        public async Task<ActionResult> GetCategory()
        {
            var result = await _dataContext.Locations.GroupBy(x => x.Category).Select(a => a.Key).ToArrayAsync();
            return Ok(result);
        }

        [HttpPut("UpdateCategoryService")]
        public async Task<ActionResult> UpdateCategory(UpdateCategoryDto dto)
        {
            var Create = new UpdateCategoryDtoValidator();
            var result = Create.Validate(dto);

            if (!result.IsValid)
            {
                var errors = result.Errors.Select(error => error.ErrorMessage).ToList();
                return BadRequest(new { Message = "Validation Create Error", Errors = errors });
            }

            return HandleResult(await _categoryLocationService.UpdateCategoryAsync(dto));
        }
    }
}
