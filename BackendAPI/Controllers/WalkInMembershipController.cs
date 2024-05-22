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

            if (user == null)
            {
                return HandleResult(Result<string>.Failure("Not Found User"));
            }

            if (dto.durationMonths <= 0)
            {
                return HandleResult(Result<string>.Failure("Falied To RegisterMembership"));
            }

            var memberprice = await _dataContext.MembershipPrices.FirstOrDefaultAsync(x => x.Id == dto.MembershipPriceId);

            if (memberprice == null)
            {
                return HandleResult(Result<string>.Failure("Not Found MemberId"));
            }

            var searchMembership = await _dataContext.WalkInMemberships.FirstOrDefaultAsync(x => x.UserId == dto.userId && x.MembershipPriceId == dto.MembershipPriceId);
            if (searchMembership != null)
            {
                return HandleResult(Result<string>.Failure("Membership for this user in this location already"));
            }

            var currentTime = DateTime.Now;
            var startTime = currentTime;
            var endTime = currentTime.AddMonths(dto.durationMonths);

            var membership = new WalkInMembership
            {
                UserId = user.Id,
                MembershipPriceId = dto.MembershipPriceId,
                CreateBy = dto.CreateBy,
                StartTime = startTime,
                EndTime = endTime,
            };

            _dataContext.WalkInMemberships.Add(membership);
            await _dataContext.SaveChangesAsync();
            return HandleResult(Result<string>.Success("Register MemberShip Success"));
        }


        [HttpPost("AddMonthsMembership")]
        public async Task<ActionResult> AddMonthsMembership(AddTimeMembershipDto dto)
        {
            var membership = await _dataContext.WalkInMemberships.Include(x => x.MembershipPrice).FirstOrDefaultAsync(x => x.UserId == dto.userId && x.MembershipPrice.LocationId == dto.locationId);

            if (membership == null)
            {
                return HandleResult(Result<string>.Failure("Membership not found"));
            }

            if (dto.months <= 0)
            {
                return HandleResult(Result<string>.Failure("Error months"));
            }

            var currentTime = DateTime.Now;

            if (membership.EndTime < currentTime)
            {
                membership.EndTime = currentTime.AddMonths(dto.months);
            }
            else
            {
                membership.EndTime = membership.EndTime.AddMonths(dto.months);
            }
            await _dataContext.SaveChangesAsync();

            return HandleResult(Result<string>.Success("Membership duration extended successfully"));
        }


        [HttpGet("GetUserMember")]
        public async Task<ActionResult> GetUserMember()
        {
            var members = await _dataContext.WalkInMemberships.Include(x => x.MembershipPrice.Location).ToListAsync();

            if (members == null)
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
                        //ExpirationTime = member.ExpirationTime,
                        StartTime = member.StartTime,
                        EndTime = member.EndTime,
                        location = member.MembershipPrice.Location,
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
                                              .Include(x => x.MembershipPrice).ThenInclude(x=>x.Location)
                                              .Where(x => x.MembershipPrice.LocationId == locationid)
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
                        //ExpirationTime = member.ExpirationTime,
                        location = member.MembershipPrice.Location,
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
                        Createby = userName,
                        MemberPriceId = member.MembershipPriceId,
                        StartTime = member.StartTime,
                        EndTime = member.EndTime,
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


        [HttpPost("CreateMembershipPrice")]
        public async Task<ActionResult> CreateMembershipPrice(CreateMembershipPriceDto dto)
        {

            var existingPrice = await _dataContext.MembershipPrices.FirstOrDefaultAsync(x => x.LocationId == dto.LocationId);
            if (existingPrice != null)
            {
                return HandleResult(Result<string>.Failure("LocationId already exists"));
            }

            var location = await _dataContext.Locations.FirstOrDefaultAsync(x => x.Id == dto.LocationId);
            if(location == null)
            {
                return HandleResult(Result<string>.Failure("Not Found Location"));
            }

            var create = new MembershipPrice
            {
                LocationId = dto.LocationId,
                PriceForMonth = dto.PriceForMonth,
                PriceWalkin = dto.PriceWalkin,
                Created = DateTime.Now,
            };

            _dataContext.MembershipPrices.Add(create);
            await _dataContext.SaveChangesAsync();
            return HandleResult(Result<string>.Success("Create Price For Member Success"));
        }


        [HttpPost("UpdateMembershipPrice")]
        public async Task<ActionResult> UpdateMembershipPrice(UpdateMembershipPriceDto dto)
        {
            var existingPrice = await _dataContext.MembershipPrices.FirstOrDefaultAsync(x => x.Id == dto.Id);
            if (existingPrice == null)
            {
                return HandleResult(Result<string>.Failure("Id MemberPrice not found"));
            }


            if (dto.LocationId != existingPrice.LocationId)
            {
                existingPrice.LocationId = dto.LocationId;
            }

            if (dto.PriceForMonth != existingPrice.PriceForMonth)
            {
                existingPrice.PriceForMonth = dto.PriceForMonth;
            }

            if (dto.PriceWalkin != existingPrice.PriceWalkin)
            {
                existingPrice.PriceWalkin = dto.PriceWalkin;
            }

            _dataContext.MembershipPrices.Update(existingPrice);
            await _dataContext.SaveChangesAsync();

            return HandleResult(Result<string>.Success("Update Price For Member Success"));
        }

        //[HttpDelete("DeleteMembershipPrice")]
        [HttpPost("DeleteMembershipPrice")]
        public async Task<ActionResult> DeleteMembershipPrice(int id)
        {
            var MemberShipPrice = await _dataContext.MembershipPrices.FindAsync(id);

            if (MemberShipPrice == null)
            {
                return HandleResult(Result<string>.Failure("Not Found MemberShipPrice"));
            }

            var MemberShipCount = await _dataContext.WalkInMemberships.CountAsync(x => x.MembershipPriceId == id);
            if (MemberShipCount > 0)
            {
                return HandleResult(Result<string>.Failure("Cannot Delete because Have Member"));
            }

            _dataContext.MembershipPrices.Remove(MemberShipPrice);
            await _dataContext.SaveChangesAsync();
            return HandleResult(Result<string>.Success("Delete Success"));
        }

        [HttpGet("GetLocationHasPriceMemberAndStatus")]
        public async Task<ActionResult> GetLocationHasPriceMemberAndStatus()
        {
            var location = await _dataContext.MembershipPrices.Include(x => x.Location).Where(x=>x.Location.StatusOnOff == 1) .ToListAsync();
            if(location == null)
            {
                return HandleResult(Result<string>.Failure("Not Fount Location"));
            }

            var result = location.Select(lp => new
            {
                Location = lp.Location,
                PriceForMonth = lp.PriceForMonth,
                PriceForDay = lp.PriceWalkin,
                Created = lp.Created,
                MemberPriceId = lp.Id,
            });

            return HandleResult(Result<object>.Success(result));
        }

        [HttpGet("GetMemberShipPrice")]
        public async Task<ActionResult>GetMemberShipPrice()
        {
            var result = await _dataContext.MembershipPrices.Include(x=>x.Location).ThenInclude(x=>x.Category).ToListAsync();
            if(result == null)
            {
                return HandleResult(Result<string>.Failure("Not Found MemberShipPrice"));
            }


            var tasks = result.Select(async x => new
            {
                id = x.Id,
                priceForMonth = x.PriceForMonth,
                priceForWalkin = x.PriceWalkin,
                locationId = x.LocationId,
                name = x.Location
            });

            var membership = await Task.WhenAll(tasks);
            return HandleResult(Result<object>.Success(membership));
        }


        [HttpGet("GetLocationsWithoutMembershipPrice")]
        public async Task<ActionResult<IEnumerable<Location>>> GetLocationsWithoutMembershipPrice()
        {
            var locationsWithoutMembershipPrice = await _dataContext.Locations
                .Where(location => !_dataContext.MembershipPrices.Any(price => price.LocationId == location.Id)).Include(x => x.Category)
                .ToListAsync();

            return Ok(locationsWithoutMembershipPrice);
        }


        [HttpGet("GetMemberShipPriceandMemberShip")]
        public async Task<ActionResult> GetMemberShipPriceandMemberShip()
        {
            var membershipPrices = await _dataContext.WalkInMemberships
                .Select(membership => new
                {
                    membership.Id,
                    membership.UserId,
                    membership.StartTime,
                    membership.EndTime,
                    membership.MembershipPriceId,
                    PriceForMonth = membership.MembershipPrice != null ? membership.MembershipPrice.PriceForMonth : 0,
                    TotalMonths = ((membership.EndTime.Year - membership.StartTime.Year) * 12) + membership.EndTime.Month - membership.StartTime.Month,
                    TotalPrice = ((membership.MembershipPrice != null ? membership.MembershipPrice.PriceForMonth : 0) * (((membership.EndTime.Year - membership.StartTime.Year) * 12) + membership.EndTime.Month - membership.StartTime.Month))

                })
                .ToListAsync();

            return Ok(membershipPrices);
        }


        [HttpGet("GetWalkInMembershipsByUserId")]
        public async Task<ActionResult<IEnumerable<object>>> GetWalkInMembershipsByUserId(string userId)
        {
            var walkInMemberships = await _dataContext.WalkInMemberships
                .Where(membership => membership.UserId == userId)
                .Include(membership => membership.MembershipPrice)
                .Join(
                    _dataContext.Locations,
                    membership => membership.MembershipPrice.LocationId,
                    location => location.Id,
                    (membership, location) => new
                    {
                        Membership = membership,
                        LocationName = location.Name
                    })
                .ToListAsync();

            return Ok(walkInMemberships);
        }






    }
}
