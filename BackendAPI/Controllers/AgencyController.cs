using BackendAPI.Core;
using BackendAPI.Data;
using BackendAPI.DTOs.AgencyDtos;
using BackendAPI.DTOs.RoomsDto;
using BackendAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackendAPI.Controllers
{
    public class AgencyController : BaseApiController
    {
        private readonly DataContext _dataContext;

        public AgencyController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        [HttpPost("CreateAgency")]
        public async Task<IActionResult> CreateAgency(CreateAgencyDto dto)
        {
            if (_dataContext.Agencys.Any(x => x.Name == dto.Name))
            {
                return HandleResult(Result<string>.Failure("Agency Name is already in use."));
            }

            var agency = new Agency
            {
                Name = dto.Name,
                CreatedDate = dto.CreatedDate,
            };
            _dataContext.Add(agency);
            await _dataContext.SaveChangesAsync();

            return HandleResult(Result<string>.Success("Category created successfully"));
        }

        [HttpGet("GetAgency")]
        public async Task<IActionResult> GetAgency()
        {
            var result = await _dataContext.Agencys.ToListAsync();
            if (result == null || result.Count == 0)
            {
                return HandleResult(Result<dynamic>.Failure("Notfound Agency"));
            }
            return HandleResult(Result<dynamic>.Success(result));
        }

        [HttpPost("UpdateAgency")]
        public async Task<ActionResult> UpdateAgency (UpdateAgencyDto dto)
        {
            var find = await _dataContext.Agencys.FindAsync(dto.Id);

            if (find == null) return HandleResult(Result<string>.Failure("Not Found AgentID "));

            if (_dataContext.Agencys.Any(x => x.Name == dto.Name))
                return HandleResult(Result<string>.Failure("Agency name have already"));

            find.Name = dto.Name;
            find.CreatedDate = dto.CreatedDate;

            await _dataContext.SaveChangesAsync();

            return HandleResult(Result<string>.Success("Update Agency Success"));
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteAgency(int id)
        {
            var agency = await _dataContext.Agencys.FindAsync(id);

            if (agency == null)
            {
                return HandleResult(Result<string>.Failure("Not Found Agency"));
            }

            _dataContext.Agencys.Remove(agency);
            await _dataContext.SaveChangesAsync();
            return HandleResult(Result<string>.Success($"Delete Agency Success"));
        }
    }
}
