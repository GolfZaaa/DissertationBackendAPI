using BackendAPI.Core;
using BackendAPI.Data;
using BackendAPI.DTOs.Cart;
using BackendAPI.DTOs.ReservationsDtos;
using BackendAPI.Models;
using BackendAPI.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json.Linq;
using Stripe;
using Stripe.Climate;
using Stripe.Issuing;

namespace BackendAPI.Controllers
{
    public class ReservationsController : BaseApiController
    {
        private readonly DataContext _dataContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMemoryCache _memoryCache;

        public ReservationsController(DataContext dataContext, UserManager<ApplicationUser> userManager, IMemoryCache memoryCache)
        {
            _dataContext = dataContext;
            _userManager = userManager;
            _memoryCache = memoryCache;
        }



        [HttpPost("CreateReservations AddToCart")]
        public async Task<ActionResult> CreateReservations(ReservationsDto dto)
        {
            var user = await _dataContext.Users.FindAsync(dto.UserId);

            //if (user == null)
            //    return HandleResult(Result<string>.Failure("User not found"));

            var location = await _dataContext.Locations.Include(x => x.Category).FirstOrDefaultAsync(l => l.Id == dto.LocationId);

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

            // การเพิ่มข้อมูลจองลงในตะกร้า

            var reservationCartItem = new CartItem
            {
                Locations = location,
                StartTime = dto.StartTime,
                EndTime = dto.EndTime,
                CountPeople = dto.CountPeople,
            };

            location.Status = 0;

            TimeSpan totalHours = reservationCartItem.EndTime - reservationCartItem.StartTime;
            double totalHoursValue = totalHours.TotalHours;

            // หากต้องการให้ผลลัพธ์เป็นจำนวนชั่วโมงทั้งหมดที่เป็นจำนวนเต็ม
            int totalRoundedHours = (int)Math.Round(totalHoursValue);

            //_dataContext.Reservations.Add(reservation);
            await _dataContext.SaveChangesAsync();



            // ดึงข้อมูลผู้ใช้และตะกร้าจากฐานข้อมูล
            var cart = await _dataContext.Carts
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.User.Id == user.Id);

            // หากไม่มีตะกร้า ให้สร้างตะกร้าใหม่
            if (cart == null)
            {
                cart = new Cart
                {
                    User = user,
                };
                _dataContext.Carts.Add(cart);
            }

            // เพิ่ม ReservationCartItem ลงในตะกร้า
            cart.Items.Add(reservationCartItem);
            await _dataContext.SaveChangesAsync();


            return HandleResult(Result<string>.Success("Add To Cart Success"));
        }



        [HttpPost("AddToCart")]
        public async Task<object> AddItemToCartAsync(AddCartDTO dto)
        {
            var carttest = await RetrieveCart(dto.userId);

            var user = await _dataContext.Users.SingleOrDefaultAsync(u => u.Id == dto.userId);

            if (user == null)
            {
                return HandleResult(Result<string>.Failure("User not found"));
            }

            var location = await _dataContext.Locations.Include(x => x.Category).FirstOrDefaultAsync(l => l.Id == dto.LocationId);

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

            location.Status = 0;

            var shopCart = _dataContext.Carts.FirstOrDefault(x => x.User.Id == dto.userId);
            if (shopCart == null)
            {
                Cart cart = new Cart { User = user };
                await _dataContext.Carts.AddAsync(cart);
                await _dataContext.SaveChangesAsync();
                shopCart = cart;
            }
            shopCart.AddItem(location, dto.CountPeople,dto.StartTime, dto.EndTime);
            try
            {
                await _dataContext.SaveChangesAsync();
                return HandleResult(Result<string>.Success("Add Product to Cart Successfuly"));

            }
            catch (DbUpdateException ex)
            {
                return HandleResult(Result<string>.Success("Fail Add Product to Cart"));
            }
        }

        private async Task<Cart> RetrieveCart(string accountId)
        {
            var cart = await _dataContext.Carts
                   .Include(i => i.Items)
                   .ThenInclude(p => p.Locations)
                   .SingleOrDefaultAsync(x => x.User.Id == accountId);
            return cart;
        }


        [HttpGet("GetCartByID")]
        public async Task<object> GetCartItemsByUserIdAsync(string userId)
        {
            var userCart = await _dataContext.Carts
                .Include(c => c.Items)
                .ThenInclude(i => i.Locations)
                .Where(c => c.User.Id == userId)
                .FirstOrDefaultAsync();

            if (userCart == null)
            {
                return HandleResult(Result<string>.Failure("Cart not found for this user"));
            }

            // userCart ตอนนี้จะมีข้อมูลของตะกร้าสินค้าของผู้ใช้ที่มี userId ที่ระบุ

            var cartItems = userCart.Items; // นี่คือรายการสินค้าทั้งหมดในตะกร้า
         
            return HandleResult(Result<object>.Success(cartItems));
        }

        [HttpDelete("DeleteItemToCart")]
        public async Task<ActionResult> DeleteItemToCart(DeleteLocationInCartDTO dto)
        {
            var userCart = await RetrieveCart(dto.UserId);
            if (userCart == null)
            {
                return HandleResult(Result<string>.Failure("Cart not found"));
            }

            var cartItem = userCart.Items.FirstOrDefault(item => item.Locations.Id == dto.LocationId);
            if (cartItem == null)
            {
                return HandleResult(Result<string>.Failure("Item not found in the cart"));
            }
            try
            {
                var locationToRemove = await _dataContext.Locations.FirstOrDefaultAsync(l => l.Id == dto.LocationId);
                if (locationToRemove != null)
                {
                    locationToRemove.Status = 1;
                }
                _dataContext.CartItems.Remove(cartItem);
                await _dataContext.SaveChangesAsync();
                return HandleResult(Result<string>.Success("Item removed from cart successfully"));
            }
            catch (DbUpdateException ex)
            {
                return HandleResult(Result<string>.Failure("Failed to remove item from cart"));
            }
        }

        //[HttpPost("UpdateItemInCart")]
        //public async Task<ActionResult> UpdateItemInCart


        //[HttpDelete("RemoveFromCart/{cartItemId}")]
        //public async Task<ActionResult> RemoveFromCart(int cartItemId)
        //{
        //    var cartItem = await _dataContext.CartItems.Include(a => a.Locations).FirstOrDefaultAsync(x => x.Id == cartItemId);

        //    var cartItem2 = await _dataContext.CartItems.FindAsync(cartItemId);

        //    if (cartItem == null)
        //    {
        //        return HandleResult(Result<string>.Failure("Cart item not found"));
        //    }
        //    cartItem.Locations.Status = 1;


        //    _dataContext.CartItems.Remove(cartItem);
        //    await _dataContext.SaveChangesAsync();

        //    return HandleResult(Result<string>.Success("Item removed from cart"));
        //}

        //[HttpGet("GetCartByID")]
        //public async Task<ActionResult> GetCartById(string userId)
        //{
        //    var currentUser = await _dataContext.Users.FindAsync(userId);

        //    if (currentUser == null)
        //    {
        //        return HandleResult(Result<string>.Failure("User not found"));
        //    }
        //    var cart = await _dataContext.Carts
        //.Include(c => c.Items)
        //.ThenInclude(x => x.Locations)
        //.ThenInclude(x => x.Category)
        //.ThenInclude(a=>a.Locations)
        //.ThenInclude(a=>a.locationImages)
        //.FirstOrDefaultAsync(c => c.User.Id == currentUser.Id);

        //    if (cart == null)
        //    {
        //        return HandleResult(Result<string>.Failure("Cart not found"));
        //    }
        //    return HandleResult(Result<object>.Success(cart));


        //}




        //[HttpGet("GetReservations")]
        //public async Task<ActionResult> GetReservations(int id)
        //{
        //    //var result = await _dataContext.Reservations.Include(p => p.Locations.locationImages).Include(x => x.Locations.Category).Include(p => p.ReservationCartItems)
        //    var result = await _dataContext.ReservationsOrders.Include(x=>x.OrderItems).ThenInclude(x=>x.Location).ThenInclude(x=>x.Category)

        //   .AsNoTracking()
        //   .FirstOrDefaultAsync(p => p.Id == id);
        //    return HandleResult(Result<ReservationsOrder>.Success(result));
        //}



        //// เป็นการเปลี่ยน Status ห้อง ให้เป็น 1 
        //[HttpGet("CheckReservationStatus")]
        //public async Task<ActionResult> CheckReservationStatus()
        //{
        //    var currentDateTime = DateTime.Now;
        //    var overdueReservations = await _dataContext.ReservationsOrders
        //        .Include(x => x.OrderItems)
        //        .ThenInclude(x=>x.Location)
        //        .Where(r => r.OrderItems.Any(x=>x.EndTime < currentDateTime))
        //        .ToListAsync();


        //    foreach (var reservation in overdueReservations)
        //    {
        //        if (reservation.StatusFinished == 1 && reservation.OrderItems.Any(x => x.Location.Status == 0))
        //        {
        //            foreach (var orderItem in reservation.OrderItems.Where(x => x.Location.Status == 0))
        //            {
        //                orderItem.Location.Status = 1;
        //            }
        //            reservation.StatusFinished = 0;
        //        }
        //    }


        //    await _dataContext.SaveChangesAsync();
        //    return HandleResult(Result<string>.Success("Reservation status checked and updated successfully"));
        //}

        //[HttpPut("UpdateReservations")]
        //public async Task<ActionResult> UpdateReservations(ReservationsDto dto)
        //{
        //    var reservationToUpdate = await _dataContext.CartItems.FindAsync(dto.Id);

        //    if (reservationToUpdate == null)
        //        return HandleResult(Result<string>.Failure("Reservation not found"));

        //    var user = await _dataContext.Users.FindAsync(dto.UserId);

        //    if (user == null)
        //        return HandleResult(Result<string>.Failure("User not found"));

        //    var location = await _dataContext.Locations.FirstOrDefaultAsync(l => l.Id == dto.LocationId);

        //    if (location == null)
        //        return HandleResult(Result<string>.Failure("Location not found"));

        //    if (dto.CountPeople > location.Capacity)
        //    {
        //        return HandleResult(Result<string>.Failure("The number of people booking exceeds the room capacity."));
        //    }

        //    reservationToUpdate.StartTime = dto.StartTime;
        //    reservationToUpdate.EndTime = dto.EndTime;
        //    reservationToUpdate.CountPeople = dto.CountPeople;
        //    reservationToUpdate.Locations = location;

        //    await _dataContext.SaveChangesAsync();

        //    return HandleResult(Result<string>.Success("Update Success"));
        //}


        //[HttpDelete]
        //public async Task<ActionResult> DeleteReservations(int id)
        //{
        //    //var reservation = await _dataContext.Reservations.FindAsync(id);

        //    var reservation = await _dataContext.ReservationsOrders.Include(x => x.OrderItems).ThenInclude(a=>a.Location).FirstOrDefaultAsync(x => x.Id == id);


        //    if (reservation == null)
        //    {
        //        return HandleResult(Result<object>.Failure("Not Found Reservation"));
        //    }

        //    foreach (var orderItem in reservation.OrderItems)
        //    {
        //        orderItem.Location.Status = 1;
        //    }

        //    _dataContext.ReservationsOrders.Remove(reservation);
        //    await _dataContext.SaveChangesAsync();

        //    return HandleResult(Result<object>.Success(reservation));
        //}

        //private string GenerateID() => Guid.NewGuid().ToString("N");




    }
}
