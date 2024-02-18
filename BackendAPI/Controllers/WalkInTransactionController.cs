using BackendAPI.Core;
using BackendAPI.Data;
using BackendAPI.DTOs;
using BackendAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackendAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalkInTransactionController : BaseApiController
    {
        private readonly DataContext _dataContext;

        public WalkInTransactionController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        [HttpPost("AddPersonWalkin")]
        public async Task<ActionResult> AddPersonWalkin(AddPersonDto dto)
        {
            var location = await _dataContext.Locations
                .Include(x => x.Category)
                .FirstOrDefaultAsync(l => l.Id == dto.LocationId);

            if (location == null)
            {
                return HandleResult(Result<string>.Failure("Location not found."));
            }

            var todayTransactions = await _dataContext.WalkInTransactions
                .Where(x => x.LocationId == dto.LocationId && x.TransactionDate.Date == DateTime.Today)
                .ToListAsync();

            var existingTransaction = todayTransactions.FirstOrDefault(x => x.UserId == dto.UserId);

            if(dto.UserId != "")
            {
                if(existingTransaction != null)
                {
                    return HandleResult(Result<string>.Failure("Duplicate user for today's transactions."));
                }
                else
                {
                    var walkInTransaction = new WalkInTransaction
                    {
                        TransactionDate = DateTime.Now,
                        NumberOfPeople = dto.NumberOfPeople,
                        LocationId = dto.LocationId,
                        UserId = dto.UserId,
                        CreateBy = dto.CreateBy,
                    };
                    _dataContext.WalkInTransactions.Add(walkInTransaction);
                }
            }
            else
            {
                var walkInTransaction = new WalkInTransaction
                {
                    TransactionDate = DateTime.Now,
                    NumberOfPeople = dto.NumberOfPeople,
                    LocationId = dto.LocationId,
                    UserId = dto.UserId,
                    CreateBy = dto.CreateBy,
                };
                _dataContext.WalkInTransactions.Add(walkInTransaction);
            }


            await _dataContext.SaveChangesAsync();
            return HandleResult(Result<string>.Success("Walk-in transaction added successfully."));
        }



      



        [HttpGet("GetWalkinByLocationId")]
        public async Task<ActionResult> GetWalkinByLocationId(int locationId)
        {
            var walkinTransactions = await _dataContext.WalkInTransactions
                                                        .Where(x => x.LocationId == locationId)
                                                        .OrderByDescending(x => x.TransactionDate) 
                                                        .ToListAsync();
            if (!walkinTransactions.Any())
            {
                return HandleResult(Result<string>.Failure("Not Found Walkin"));
            }

            var walkin = new List<object>();
            foreach (var walkinTransaction in walkinTransactions)
            {
                var userName = await GetUserName(walkinTransaction.UserId);
                var createBy = await GetUserName(walkinTransaction.CreateBy);
                var walkinItem = new
                {
                    Id = walkinTransaction.Id,
                    LocationId = await GetNameLocation(locationId),
                    UserName = userName,
                    NumberOfPeople = walkinTransaction.NumberOfPeople,
                    TransactionsDate = walkinTransaction.TransactionDate,
                    CreateBy = createBy,
                };
                walkin.Add(walkinItem);
            }

            return HandleResult(Result<object>.Success(walkin));
        }



        private async Task<string> GetUserName(string UserId)
        {
            var user = await _dataContext.Users
                .Where(x => x.Id == UserId)
                .Select(x => x.FirstName + " " + x.LastName)
                .FirstOrDefaultAsync();
            return user;
        }

        private async Task<string> GetNameLocation(int locationId)
        {
            var location = await _dataContext.Locations
                .Where(x => x.Id == locationId)
                .Select(x => x.Name)
                .FirstOrDefaultAsync();

            return location;
        }


    }
}
