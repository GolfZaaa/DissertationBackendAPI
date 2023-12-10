using BackendAPI.Core;
using BackendAPI.Data;
using BackendAPI.DTOs.ReservationsDtos;
using BackendAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json.Linq;
using Stripe;

namespace BackendAPI.Controllers
{
    public class ReservationsController : BaseApiController
    {
        private readonly DataContext _dataContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMemoryCache _memoryCache;

        public ReservationsController(DataContext dataContext,UserManager<ApplicationUser> userManager, IMemoryCache memoryCache)
        {
            _dataContext = dataContext;
            _userManager = userManager;
            _memoryCache = memoryCache;
        }



        [HttpPost("CreateReservations AddToCart")]
        public async Task<ActionResult> CreateReservations(ReservationsDto dto)
        {
            var user = await _dataContext.Users.FindAsync(dto.UserId);

            if (user == null)
                return HandleResult(Result<string>.Failure("User not found"));

            var location = await _dataContext.Locations.Include(x=>x.Category).FirstOrDefaultAsync(l => l.Id == dto.LocationId);

            if (location == null)
                return HandleResult(Result<string>.Failure("Location not found"));

            if (dto.CountPeople > location.Capacity)
            {
                return HandleResult(Result<string>.Failure("The number of people booking exceeds the room capacity."));
            }

            if (location.Status == 0)
            {
                return HandleResult(Result<string>.Failure("The room is not available"));
            }

            var reservation = new Reservations
            {
                DateTimeCreateReservations = dto.DateTimeCreateReservations,
                StartTime = dto.StartTime,
                EndTime = dto.EndTime,
                CountPeople = dto.CountPeople,
                Price = dto.Price,
                Users = user,
                Locations = location,
                StatusFinished = 1,
            };
            location.Status = 0;

            TimeSpan totalHours = reservation.EndTime - reservation.StartTime;
            double totalHoursValue = totalHours.TotalHours;

            // หากต้องการให้ผลลัพธ์เป็นจำนวนชั่วโมงทั้งหมดที่เป็นจำนวนเต็ม
            int totalRoundedHours = (int)Math.Round(totalHoursValue);

            //_dataContext.Reservations.Add(reservation);
            await _dataContext.SaveChangesAsync();

            // การเพิ่มข้อมูลจองลงในตะกร้า

            var reservationCartItem = new ReservationCartItem
            {
                Locations = location,
                TotalHour = totalRoundedHours,
                TotalPrice = totalRoundedHours * location.Category.Servicefees,
            };

            // ดึงข้อมูลผู้ใช้และตะกร้าจากฐานข้อมูล
            var cart = await _dataContext.ReservationCarts
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.User.Id == user.Id);

            // หากไม่มีตะกร้า ให้สร้างตะกร้าใหม่
            if (cart == null)
            {
                cart = new ReservationCart
                {
                    User = user,
                };
                _dataContext.ReservationCarts.Add(cart);
            }

            // เพิ่ม ReservationCartItem ลงในตะกร้า
            cart.Items.Add(reservationCartItem);
            await _dataContext.SaveChangesAsync();


            return HandleResult(Result<string>.Success("Add To Cart Success"));
        }


        [HttpDelete("RemoveFromCart/{cartItemId}")]
        public async Task<ActionResult> RemoveFromCart(int cartItemId)
        {
            var cartItem = await _dataContext.ReservationCartItems.FindAsync(cartItemId);

            if (cartItem == null)
            {
                return HandleResult(Result<string>.Failure("Cart item not found"));
            }

            _dataContext.ReservationCartItems.Remove(cartItem);
            await _dataContext.SaveChangesAsync();

            return HandleResult(Result<string>.Success("Item removed from cart"));
        }

        [HttpGet("GetCartByID")]
        public async Task<ActionResult> GetCartById (string userId)
        {
            var currentUser = await _dataContext.Users.FindAsync(userId);

            if (currentUser == null)
            {
                return HandleResult(Result<string>.Failure("User not found"));
            }
            var cart = await _dataContext.ReservationCarts
        .Include(c => c.Items)
        .ThenInclude(x=>x.Locations)
        .ThenInclude(x=>x.Category)
        .FirstOrDefaultAsync(c => c.User.Id == currentUser.Id);

            if (cart == null)
            {
                return HandleResult(Result<string>.Failure("Cart not found"));
            }
            return HandleResult(Result<object>.Success(cart));


        }




        [HttpGet("GetReservations")]
        public async Task<ActionResult> GetReservations (int id)
        {
        var result = await _dataContext.Reservations.Include(p => p.Locations.locationImages).Include(x=>x.Locations.Category).Include(p => p.Users)
       .AsNoTracking()
       .FirstOrDefaultAsync(p => p.Id == id);
        return HandleResult(Result<Reservations>.Success(result));
        }



        // เป็นการเปลี่ยน Status ห้อง ให้เป็น 1 
        [HttpGet("CheckReservationStatus")]
        public async Task<ActionResult> CheckReservationStatus()
        {
            var currentDateTime = DateTime.Now;
            var overdueReservations = await _dataContext.Reservations
                .Include(r => r.Locations) 
                .Where(r => r.EndTime < currentDateTime)
                .ToListAsync();


            foreach (var reservation in overdueReservations)
            {
                if(reservation.StatusFinished == 1)
                {
                    if (reservation.Locations.Status == 0)
                    {
                        reservation.Locations.Status = 1;
                        reservation.StatusFinished = 0;
                    }
                }
            }
            await _dataContext.SaveChangesAsync();
            return HandleResult(Result<string>.Success("Reservation status checked and updated successfully"));
        }

        [HttpPut("UpdateReservations")]
        public async Task<ActionResult> UpdateReservations(ReservationsDto dto)
        {
            var reservationToUpdate = await _dataContext.Reservations.FindAsync(dto.Id);

            if (reservationToUpdate == null)
                return HandleResult(Result<string>.Failure("Reservation not found"));

            var user = await _dataContext.Users.FindAsync(dto.UserId);
            
            if (user == null)
                return HandleResult(Result<string>.Failure("User not found"));

            var location = await _dataContext.Locations.FirstOrDefaultAsync(l => l.Id == dto.LocationId);

            if (location == null)
                return HandleResult(Result<string>.Failure("Location not found"));

            if (dto.CountPeople > location.Capacity)
            {
                return HandleResult(Result<string>.Failure("The number of people booking exceeds the room capacity."));
            }

            reservationToUpdate.DateTimeCreateReservations = dto.DateTimeCreateReservations;
            reservationToUpdate.StartTime = dto.StartTime;
            reservationToUpdate.EndTime = dto.EndTime;
            reservationToUpdate.CountPeople = dto.CountPeople;
            reservationToUpdate.Price = dto.Price;
            reservationToUpdate.Users = user;
            reservationToUpdate.Locations = location;

            await _dataContext.SaveChangesAsync();

            return HandleResult(Result<string>.Success("Update Success"));
        }


        [HttpDelete]
        public async Task<ActionResult> DeleteReservations (int id)
        {
            //var reservation = await _dataContext.Reservations.FindAsync(id);

            var reservation = await _dataContext.Reservations.Include(x => x.Locations).FirstOrDefaultAsync(x=>x.Id == id);


            if (reservation == null)
            {
                return HandleResult(Result<object>.Failure("Not Found Reservation"));
            }

            if (reservation.Locations == null)
            {
                return HandleResult(Result<object>.Failure("Not Found Locations"));
            }

            reservation.Locations.Status = 1; 
            _dataContext.Reservations.Remove(reservation);
            await _dataContext.SaveChangesAsync();

            return HandleResult(Result<object>.Success(reservation));
        }



        //private async Task<PaymentIntent> CreatePaymentIntent()

    }
}
