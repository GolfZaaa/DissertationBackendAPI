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
                CreatedDate = DateTime.Now,
            };
            _dataContext.Add(agency);
            await _dataContext.SaveChangesAsync();

            return HandleResult(Result<string>.Success("Category created successfully"));
        }

        [HttpGet("GetAgency")]
        public async Task<IActionResult> GetAgency()
        {
            var result = await _dataContext.Agencys.ToListAsync();
            return HandleResult(Result<dynamic>.Success(result.Any() ? result : new List<Agency>()));
        }


        [HttpPost("UpdateAgency")]
        public async Task<ActionResult> UpdateAgency (UpdateAgencyDto dto)
        {
            var find = await _dataContext.Agencys.FindAsync(dto.Id);

            if (find == null) return HandleResult(Result<string>.Failure("Not Found AgentID "));

            if (find.Name == dto.Name) return HandleResult(Result<string>.Failure("The current name of the organization."));

            if (_dataContext.Agencys.Any(x => x.Name == dto.Name))
                return HandleResult(Result<string>.Failure("Agency name have already"));

            find.Name = dto.Name;
            find.CreatedDate = dto.CreatedDate;

            await _dataContext.SaveChangesAsync();

            return HandleResult(Result<string>.Success("Update Agency Success"));
        }

        [HttpDelete("DeleteAgency")]
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

        [HttpGet("SearchAgencyById")]
        public async Task<ActionResult> SearchAgencyById(int id)
        {
            var result = await _dataContext.Agencys.FirstOrDefaultAsync(x =>x.Id == id);

            if(result == null)
            {
                return HandleResult(Result<string>.Failure("NotFound Agency"));
            }

            return HandleResult(Result<object>.Success(result));
        }


        [HttpPost("TurnOnOffAgency")]
        public async Task<ActionResult> TurnOnOffAgency(TurnOnOffAgencyDto dto)
        {
            var agency = await _dataContext.Agencys.FirstOrDefaultAsync(x => x.Id == dto.Id);
            if (agency == null)
            {
                HandleResult(Result<string>.Failure("Not Found Agency"));
            }

            var agencyUsersCount = await _dataContext.Users.CountAsync(u => u.AgencyId == agency.Id);
            if (agencyUsersCount > 0)
            {
                return BadRequest("Cannot change status. Agency is in use by users.");
            }

            if (dto.StatusOnOff == 0)
            {
                agency.StatusOnOff = 0;
            }
            else
            {
                agency.StatusOnOff = 1;
            }
            await _dataContext.SaveChangesAsync();
            return Ok(agency);
        }

        [HttpGet("GetAgencyStatus")]
        public async Task<ActionResult> GetAgencyStatus()
        {
            var Agency = await _dataContext.Agencys.Where(x => x.StatusOnOff == 1).ToListAsync();
            if (Agency == null)
            {
                HandleResult(Result<string>.Failure("Not Found Agency"));
            }
            return Ok(Agency);
        }

    }
}
