using AutoMapper.Execution;
using BackendAPI.Core;
using BackendAPI.Data;
using BackendAPI.DTOs.MemberShipDtos;
using BackendAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackendAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalkInMembershipController : BaseApiController
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly DataContext _dataContext;

        public WalkInMembershipController(UserManager<ApplicationUser> userManager, DataContext dataContext)
        {
            _userManager = userManager;
            _dataContext = dataContext;
        }

        [HttpPost("RegisterMembership")]
        public async Task<ActionResult> RegisterMembership(RegisterMemberDto dto)
        {
            var user = await _dataContext.Users.FirstOrDefaultAsync(x => x.Id == dto.userId);

            if(user == null)
            {
                return HandleResult(Result<string>.Failure("Not Found User"));
            }

            if (dto.durationMonths <= 0)
            {
                return HandleResult(Result<string>.Failure("Falied To RegisterMembership"));
            }


            var searchMembership = await _dataContext.WalkInMemberships.FirstOrDefaultAsync(x => x.UserId == dto.userId && x.LocationId == dto.LocationId);
            if (searchMembership != null)
            {
                return HandleResult(Result<string>.Failure("Membership for this user in this location already"));
            }

           

            var expirationTime = DateTime.UtcNow.AddMonths(dto.durationMonths);

            var membership = new WalkInMembership
            {
                UserId = user.Id,
                ExpirationTime = expirationTime,
                LocationId = dto.LocationId,
                CreateBy = dto.CreateBy,
            };

            _dataContext.WalkInMemberships.Add(membership);
            await _dataContext.SaveChangesAsync();
            return HandleResult(Result<string>.Success("Register MemberShip Success"));
        }

        [HttpPost("AddMonthsMembership")]
        public async Task<ActionResult> AddMonthsMembership(AddTimeMembershipDto dto)
        {
            var membership = await _dataContext.WalkInMemberships.FirstOrDefaultAsync(x => x.UserId == dto.userId && x.LocationId == dto.locationId);

            if (membership == null)
            {
                return HandleResult(Result<string>.Failure("Membership not found"));
            }

            if (dto.months <= 0)
            {
                return HandleResult(Result<string>.Failure("Error months"));
            }

            membership.ExpirationTime = membership.ExpirationTime.AddMonths(dto.months);
            await _dataContext.SaveChangesAsync();

            return HandleResult(Result<string>.Success("Membership duration extended successfully"));
        }

        [HttpGet("GetUserMember")]
        public async Task<ActionResult> GetUserMember()
        {
            var members = await _dataContext.WalkInMemberships.Include(x=>x.Location).ToListAsync();

            if(members == null)
            {
                return HandleResult(Result<string>.Failure("Not Found Member"));
            }
            var users = await _dataContext.Users
                   .Where(u => members.Select(m => m.UserId).Contains(u.Id))
                   .Select(u => new
                   {
                       u.Email,
                       u.FirstName,
                       u.LastName,
                       u.PhoneNumber,
                       u.AgencyId,
                       u.ProfileImage,
                       u.UserName,
                       u.Id,
                   })
            .ToListAsync();

            var result = new List<object>();

            foreach (var member in members)
            {
                var userName = await GetUserName(member.CreateBy);
                var user = users.FirstOrDefault(u => u.Id == member.UserId);
                if (user != null)
                {
                    result.Add(new
                    {
                        MemberId = member.Id,
                        ExpirationTime = member.ExpirationTime,
                        location = member.Location,
                        UserData = new
                        {
                            user.Email,
                            user.FirstName,
                            user.LastName,
                            user.PhoneNumber,
                            user.AgencyId,
                            user.ProfileImage,
                            user.UserName,
                            user.Id,
                        },
                        Create = userName
                    });
                }
            }


            return HandleResult(Result<object>.Success(result));
        }

        [HttpGet("GetUserMemberByLocationId")]
        public async Task<ActionResult> GetUserMemberByLocationId(int locationid)
        {
            var members = await _dataContext.WalkInMemberships
                                              .Include(x => x.Location)
                                              .Where(x => x.LocationId == locationid)
                                              .ToListAsync();

            if (members == null || members.Count == 0)
            {
                return HandleResult(Result<string>.Failure("No members found for the specified location"));
            }

            var userIds = members.Select(m => m.UserId).ToList();

            var users = await _dataContext.Users
                                           .Where(u => userIds.Contains(u.Id))
                                           .Select(u => new
                                           {
                                               u.Email,
                                               u.FirstName,
                                               u.LastName,
                                               u.PhoneNumber,
                                               u.AgencyId,
                                               u.ProfileImage,
                                               u.UserName,
                                               u.Id,
                                           })
                                           .ToListAsync();

            var result = new List<object>();

            foreach (var member in members)
            {
                var userName = await GetUserName(member.CreateBy);
                var user = users.FirstOrDefault(u => u.Id == member.UserId);
                if (user != null)
                {
                    result.Add(new
                    {
                        MemberId = member.Id,
                        ExpirationTime = member.ExpirationTime,
                        location = member.Location,
                        UserData = new
                        {
                            user.Email,
                            user.FirstName,
                            user.LastName,
                            user.PhoneNumber,
                            user.AgencyId,
                            user.ProfileImage,
                            user.UserName,
                            user.Id,
                        },
                        Createby = userName
                    });
                }
            }

            return HandleResult(Result<object>.Success(result));
        }


        private async Task<string> GetUserName(string UserId)
        {
            var user = await _dataContext.Users
                .Where(x => x.Id == UserId)
                .Select(x => x.FirstName + " " + x.LastName)
                .FirstOrDefaultAsync();
            return user;
        }

    }
}
