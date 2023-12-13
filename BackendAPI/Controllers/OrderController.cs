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

        [HttpPost]
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
                StatusFinished = 1,
                
            };


            foreach (var item in cart.Items)
            {

                TimeSpan totalHours = item.EndTime - item.StartTime;
                double totalHoursValue = totalHours.TotalHours;
                // หากต้องการให้ผลลัพธ์เป็นจำนวนชั่วโมงทั้งหมดที่เป็นจำนวนเต็ม
                int totalRoundedHours = (int)Math.Round(totalHoursValue);

                // ทบทวนอีกที
                var text = await _dataContext.ReservationsOrderItems.Include(x => x.Location).ThenInclude(x => x.Category).FirstOrDefaultAsync();


                var orderItem = new ReservationsOrderItem
                {
                    LocationId = item.Locations.Id,
                    StartTime = item.StartTime,
                    EndTime = item.EndTime,
                    ReservationsOrder = order,
                    Price = text.Location.Category.Servicefees * totalRoundedHours
                };
                order.OrderItems.Add(orderItem);
            }
            _dataContext.ReservationsOrders.Add(order);

            var location = await _dataContext.ReservationsOrderItems.Include(x => x.Location)
                .FirstOrDefaultAsync(x => x.LocationId == x.LocationId);

            if(location != null)
            {
                location.Location.Status = 0;
            }

            await _dataContext.SaveChangesAsync();
            _dataContext.Carts.RemoveRange(cart);


            return HandleResult(Result<Object>.Success(order));

        }

        private async Task<Cart> RetrieveCart(string accountId)
        {
            var cart = await _dataContext.Carts
                   .Include(i => i.Items)
                   .ThenInclude(p => p.Locations)
                   .SingleOrDefaultAsync(x => x.User.Id == accountId);
            return cart;
        }

    }
}
