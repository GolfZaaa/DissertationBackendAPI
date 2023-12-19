using BackendAPI.Core;
using BackendAPI.Data;
using BackendAPI.DTOs.OrderDtos;
using BackendAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackendAPI.Controllers
{
    public class OrderController : BaseApiController
    {
        private readonly DataContext _dataContext;

        public OrderController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        [HttpPost("CreateOrder")]
        public async Task<ActionResult> CreateOrder([FromForm]OrderDto dto)
        {
            var user = await _dataContext.Users.SingleOrDefaultAsync(u => u.Id == dto.UserId);

            if(user == null)
               return HandleResult(Result<string>.Failure("User Not Found"));

            var cart = await RetrieveCart(dto.UserId);

            if (cart == null)
                return HandleResult(Result<string>.Failure("Cart Not Found"));

            var order = new ReservationsOrder
            {
                OrderDate = DateTime.Now,
                OrderStatus = OrderStatus.PendingApproval,
            };

            foreach (var item in cart.Items)
            {
                var locationtest = await _dataContext.Locations
            .Include(x => x.Category)
            .FirstOrDefaultAsync(x => x.Id == item.Locations.Id);

                if (locationtest == null || locationtest.Category == null)
                {
                    return HandleResult(Result<string>.Failure("Location or Category Not Found"));
                }

                TimeSpan totalHours = item.EndTime - item.StartTime;
                double totalHoursValue = totalHours.TotalHours;
                // หากต้องการให้ผลลัพธ์เป็นจำนวนชั่วโมงทั้งหมดที่เป็นจำนวนเต็ม
                int totalRoundedHours = (int)Math.Round(totalHoursValue);

                long price = totalRoundedHours * locationtest.Category.Servicefees;

                var orderItem = new ReservationsOrderItem
                {
                    LocationId = item.Locations.Id,
                    StartTime = item.StartTime,
                    EndTime = item.EndTime,
                    ReservationsOrder = order,
                    Price = price,
                    StatusFinished = 1,
                };
                order.OrderItems.Add(orderItem);



                var locationstatus = await _dataContext.Locations.FirstOrDefaultAsync(X=>X.Id == item.Locations.Id);
                if(locationstatus != null)
                {
                    locationstatus.Status = 0;

                }

            }
            _dataContext.ReservationsOrders.Add(order);
            _dataContext.Carts.RemoveRange(cart);
            _dataContext.CartItems.RemoveRange(cart.Items);

            await _dataContext.SaveChangesAsync();

            return HandleResult(Result<object>.Success(order));
        }

        [HttpGet("GetOrderById")]
        public async Task<ActionResult> GetOrderById(string UserId)
        {
            var user = await _dataContext.Users.SingleOrDefaultAsync(u => u.Id == UserId);

            if (user == null)
                return HandleResult(Result<string>.Failure("User Not Found"));

            var result = await _dataContext.ReservationsOrders.ToListAsync();

            if (result == null || result.Count == 0)
            {
                return HandleResult(Result<string>.Failure("Notfound Order"));
            }

            return HandleResult(Result<object>.Success(result));
        }

        [HttpDelete("DeleteOrderById")]
        public async Task<ActionResult> DeleteOrderById(int id)
        {
            var reservation = await _dataContext.ReservationsOrders.Include(x => x.OrderItems).ThenInclude(a => a.Location).FirstOrDefaultAsync(x => x.Id == id);


            if (reservation == null)
            {
                return HandleResult(Result<object>.Failure("Not Found Reservation"));
            }

            //foreach (var orderItem in reservation.OrderItems)
            //{
            //    orderItem.Location.Status = 1;
            //}


            _dataContext.ReservationsOrders.Remove(reservation);
            await _dataContext.SaveChangesAsync();

            return HandleResult(Result<object>.Success(reservation));

        }


        // เป็นการเปลี่ยน Status ห้อง ให้เป็น 1 
        [HttpGet("CheckOrderStatus")]
        public async Task<ActionResult> CheckReservationStatus()
        {
            var currentDateTime = DateTime.Now;
            //var overdueReservations = await _dataContext.ReservationsOrders
            //    .Include(x => x.OrderItems)
            //    .ThenInclude(x => x.Location)
            //    .Where(r => r.OrderItems.Any(x => x.EndTime < currentDateTime))
            //    .ToListAsync();

            var overdueReservations = await _dataContext.ReservationsOrderItems
                    .Include(x => x.Location)
                    .Where(x => x.EndTime < currentDateTime)
                    .ToListAsync();

            foreach (var reservationItem in overdueReservations)
            {
                // Check if the location status is 0 and update it to 1
                if (reservationItem.Location.Status == 0)
                {
                    reservationItem.Location.Status = 1;
                }

                // Update the reservation status and set StatusFinished to 0
                reservationItem.StatusFinished = 0;
            }
            await _dataContext.SaveChangesAsync();
            return HandleResult(Result<string>.Success("Reservation status checked and updated successfully"));
        }

        [HttpGet("GetOrderReservationsByCategoryId")]
        public async Task<ActionResult> GetOrderReservationsByCategoryId(int id)
        {
            var reservationsOrderitem = await _dataContext.ReservationsOrderItems.Include(x => x.Location).ThenInclude(x => x.Category)
                .Where(x => x.Location.Category.Id == id).ToListAsync();

            if(reservationsOrderitem == null || !reservationsOrderitem.Any())
            {
                return HandleResult(Result<string>.Failure("No ResesrvationsOrderItem found for Category"));
            }

            return Ok(reservationsOrderitem);
        }

        private async Task<Cart> RetrieveCart(string accountId)
        {
            var cart = await _dataContext.Carts
                   .Include(i => i.Items)
                   .ThenInclude(p => p.Locations)
                   .SingleOrDefaultAsync(x => x.User.Id == accountId);
            return cart;
        }

        [HttpGet("GetReservationsOrderItem")]
        public async Task<ActionResult> GetReservationsOrderItem()
        {
            var result = await _dataContext.ReservationsOrderItems.ToListAsync();

            return HandleResult(Result<object>.Success(result));
        }

    }
}
