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



      

        //Success 

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

        //Success 




        [HttpGet("GetDailyWalkinSummaryByLocationId")]
        public async Task<ActionResult> GetDailyWalkinSummaryByLocationId(int locationId)
        {
            var dailySummary = await _dataContext.WalkInTransactions
                                                .Where(x => x.LocationId == locationId)
                                                .GroupBy(x => x.TransactionDate.Date)
                                                .Select(g => new
                                                {
                                                    Date = g.Key,
                                                    Memberpeople = g.Sum(x => !string.IsNullOrEmpty(x.UserId) ? x.NumberOfPeople : 0),
                                                    Numberpeople = g.Sum(x => string.IsNullOrEmpty(x.UserId) ? x.NumberOfPeople : 0),
                                                    TotalNumberOfPeople = g.Sum(x => x.NumberOfPeople)
                                                })
                                                .OrderByDescending(x => x.Date)
                                                .ToListAsync();

            if (!dailySummary.Any())
            {
                return HandleResult(Result<string>.Failure("No walk-in transactions found for the specified location."));
            }

            return HandleResult(Result<object>.Success(dailySummary));
        }


        [HttpGet("GetDailyWalkinSummary")]
        public async Task<ActionResult> GetDailyWalkinSummary()
        {
            var dailySummary = await _dataContext.WalkInTransactions
                                                .GroupBy(x => new { x.LocationId, x.TransactionDate.Date })
                                                .ToListAsync();

            var result = new List<object>();

            foreach (var group in dailySummary)
            {
                var locationId = group.Key.LocationId;
                var date = group.Key.Date;

                var locationName = await _dataContext.Locations
                                                     .Where(l => l.Id == locationId)
                                                     .Select(l => l.Name)
                                                     .FirstOrDefaultAsync();

                var locationImage = await _dataContext.Locations
                                                    .Where(l => l.Id == locationId)
                                                    .Select(l => l.Image)
                                                    .FirstOrDefaultAsync();

                var memberpeople = group.Sum(x => !string.IsNullOrEmpty(x.UserId) ? x.NumberOfPeople : 0);
                var numberpeople = group.Sum(x => string.IsNullOrEmpty(x.UserId) ? x.NumberOfPeople : 0);
                var totalNumberOfPeople = group.Sum(x => x.NumberOfPeople);

                result.Add(new
                {
                    LocationName = locationName,
                    LocationImage = locationImage,
                    Date = date,
                    Memberpeople = memberpeople,
                    Numberpeople = numberpeople,
                    TotalNumberOfPeople = totalNumberOfPeople
                });
            }

            if (!result.Any())
            {
                return HandleResult(Result<string>.Failure("No walk-in transactions found."));
            }

            return HandleResult(Result<object>.Success(result));
        }

        [HttpGet("GetDailyWalkinSummaryNoMemberOnly")]
        public async Task<ActionResult> GetDailyWalkinSummaryNoMemberOnly()
        {
            var dailySummary = await _dataContext.WalkInTransactions
                                                .GroupBy(x => new { x.LocationId, x.TransactionDate.Date })
                                                .ToListAsync();

            var result = new List<object>();

            foreach (var group in dailySummary)
            {
                var locationId = group.Key.LocationId;
                var date = group.Key.Date;

                var locationName = await _dataContext.Locations
                                                     .Where(l => l.Id == locationId)
                                                     .Select(l => l.Name)
                                                     .FirstOrDefaultAsync();

                

                var locationImage = await _dataContext.Locations
                                                    .Where(l => l.Id == locationId)
                                                    .Select(l => l.Image)
                                                    .FirstOrDefaultAsync();

           //     var category = await _dataContext.Locations
           //.Where(l => l.Id == locationId)
           //.Select(l => l.Category.Servicefees)
           //.FirstOrDefaultAsync();

                var priceforday = await _dataContext.MembershipPrices
                    .Where(l => l.LocationId == locationId)
                    .Select(l => l.PriceWalkin)
                    .FirstOrDefaultAsync();


                var numberpeople = group.Sum(x => string.IsNullOrEmpty(x.UserId) ? x.NumberOfPeople : 0);
                var totalNumberOfPeople = group.Sum(x => x.NumberOfPeople);

                if(numberpeople != 0)
                {
                    result.Add(new
                    {
                        LocationName = locationName,
                        LocationImage = locationImage,
                        Date = date,
                        Servicefrees = priceforday,
                        Numberpeople = numberpeople,
                        TotalNumberOfPeople = totalNumberOfPeople
                    });
                }
            }

            if (!result.Any())
            {
                return HandleResult(Result<string>.Failure("No walk-in transactions found."));
            }

            return HandleResult(Result<object>.Success(result));
        }

        [HttpGet("GetDailyWalkinTotalPeople")]
        public async Task<ActionResult> GetDailyWalkinTotalPeople()
        {
            var dailySummary = await _dataContext.WalkInTransactions
                                            .GroupBy(x => new { x.LocationId, x.TransactionDate.Date })
                                            .Select(g => new
                                            {
                                                Date = g.Key.Date,
                                                LocationData = g.GroupBy(x => x.LocationId)
                                                               .Select(g => new
                                                               {
                                                                   LocationId = g.Key,
                                                                   TotalNumberOfPeople = g.Sum(x => x.NumberOfPeople),
                                                                   Transactions = g.ToList()
                                                               })
                                            })
                                            .ToListAsync();

            var result = new List<object>();

            foreach (var dayGroup in dailySummary)
            {
                var date = dayGroup.Date;

                foreach (var locationData in dayGroup.LocationData)
                {
                    var locationId = locationData.LocationId;
                    var locationName = await _dataContext.Locations
                                                         .Where(l => l.Id == locationId)
                                                         .Select(l => l.Name)
                                                         .FirstOrDefaultAsync();

                    var locationImage = await _dataContext.Locations
                                                        .Where(l => l.Id == locationId)
                                                        .Select(l => l.Image)
                                                        .FirstOrDefaultAsync();

                    var memberpeople = locationData.Transactions.Sum(x => !string.IsNullOrEmpty(x.UserId) ? x.NumberOfPeople : 0);
                    var numberpeople = locationData.Transactions.Sum(x => string.IsNullOrEmpty(x.UserId) ? x.NumberOfPeople : 0);
                    var totalNumberOfPeople = locationData.TotalNumberOfPeople;

                    result.Add(new
                    {
                        LocationName = locationName,
                        LocationImage = locationImage,
                        Date = date,
                        Memberpeople = memberpeople,
                        Numberpeople = numberpeople,
                        TotalNumberOfPeople = totalNumberOfPeople
                    });
                }
            }

            if (!result.Any())
            {
                return HandleResult(Result<string>.Failure("No walk-in transactions found."));
            }

            return HandleResult(Result<object>.Success(result));
        }

    }
}
