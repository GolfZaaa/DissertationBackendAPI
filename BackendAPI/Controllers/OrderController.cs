using BackendAPI.Core;
using BackendAPI.Data;
using BackendAPI.DTOs.OrderDtos;
using BackendAPI.Models;
using BackendAPI.Services.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SendGrid;
using SendGrid.Helpers.Mail;
using Stripe;
using Stripe.Climate;

namespace BackendAPI.Controllers
{
    public class OrderController : BaseApiController
    {
        private readonly DataContext _dataContext;
        private readonly IConfiguration _configuration;
        private readonly IUploadFileSingleService _uploadFileSingleService;
        private readonly SendGridClient _sendGridClient;

        public OrderController(DataContext dataContext, IConfiguration configuration, IUploadFileSingleService uploadFileSingleService, SendGridClient sendGridClient)
        {
            _dataContext = dataContext;
            _configuration = configuration;
            _uploadFileSingleService = uploadFileSingleService;
            _sendGridClient = sendGridClient;
        }
        public List<ReservationsOrderItem> OrderItems { get; set; } = new List<ReservationsOrderItem>();

        //[HttpPost("CreateOrder")]
        //public async Task<ActionResult> CreateOrder([FromForm]OrderDto dto)
        //{
        //    var user = await _dataContext.Users.SingleOrDefaultAsync(u => u.Id == dto.UserId);

        //    if(user == null)
        //       return HandleResult(Result<string>.Failure("User Not Found"));

        //    var cart = await RetrieveCart(dto.UserId);

        //    if (cart == null)
        //        return HandleResult(Result<string>.Failure("Cart Not Found"));

        //    var order = new ReservationsOrder
        //    {
        //        OrderDate = DateTime.Now,
        //        //OrderStatus = OrderStatus.PendingApproval,
        //    };

        //    foreach (var item in cart.Items)
        //    {
        //        var locationtest = await _dataContext.Locations
        //    .Include(x => x.Category)
        //    .FirstOrDefaultAsync(x => x.Id == item.Locations.Id);

        //        if (locationtest == null || locationtest.Category == null)
        //        {
        //            return HandleResult(Result<string>.Failure("Location or Category Not Found"));
        //        }

        //        TimeSpan totalHours = item.EndTime - item.StartTime;
        //        double totalHoursValue = totalHours.TotalHours;
        //        // หากต้องการให้ผลลัพธ์เป็นจำนวนชั่วโมงทั้งหมดที่เป็นจำนวนเต็ม
        //        int totalRoundedHours = (int)Math.Round(totalHoursValue);

        //        long price = totalRoundedHours * locationtest.Category.Servicefees;

        //        var orderItem = new ReservationsOrderItem
        //        {
        //            LocationId = item.Locations.Id,
        //            StartTime = item.StartTime,
        //            EndTime = item.EndTime,
        //            ReservationsOrder = order,
        //            Price = price,
        //            StatusFinished = 1,
        //        };
        //        order.OrderItems.Add(orderItem);



        //        var locationstatus = await _dataContext.Locations.FirstOrDefaultAsync(X=>X.Id == item.Locations.Id);
        //        if(locationstatus != null)
        //        {
        //            locationstatus.Status = 0;

        //        }

        //    }
        //    _dataContext.ReservationsOrders.Add(order);
        //    _dataContext.Carts.RemoveRange(cart);
        //    _dataContext.CartItems.RemoveRange(cart.Items);

        //    await _dataContext.SaveChangesAsync();

        //    return HandleResult(Result<object>.Success(order));
        //}



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

        //[HttpGet]
        //public async Task<ActionResult> TestOrderById(int OrderId)
        //{
        //    var order = await _dataContext.ReservationsOrders.Include(x => x.OrderItems).FirstOrDefaultAsync(x => x.Id.Equals(OrderId));
        //    var user = await _dataContext.Users.FirstOrDefaultAsync(x => x.Id.Equals(order.UserId));

        //    return Ok(new
        //    {
        //        order = new
        //        {
        //            order.Id,
        //        },
        //        orderItem = order.OrderItems.Select(x => new
        //        {
        //            x.StartTime,
        //            x.EndTime
        //        }),
        //        user = new
        //        {
        //            user.FirstName,
        //            user.LastName
        //        },
        //    }); ;
        //}


        [HttpGet("GetOrderReservationsByCategoryId")]
        public async Task<ActionResult> GetOrderReservationsByCategoryId(int id)
        {
            var reservationsOrderitem = await _dataContext.ReservationsOrderItems.Include(x => x.Location).ThenInclude(x => x.Category)
                .Where(x => x.Location.Category.Id == id).OrderBy(x=>x.StartTime).ToListAsync();

            if (reservationsOrderitem == null || !reservationsOrderitem.Any())
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
            var result = await _dataContext.ReservationsOrderItems.Include(x => x.Location).ToListAsync();

            return HandleResult(Result<object>.Success(result));
        }


        private async Task<(string errorMessge, string imageNames)> UploadImageMainAsync(IFormFile formfile)
        {
            var errorMessge = string.Empty;
            var imageName = string.Empty;

            if (_uploadFileSingleService.IsUpload(formfile))
            {
                errorMessge = _uploadFileSingleService.Validation(formfile);
                if (errorMessge is null)
                {
                    imageName = await _uploadFileSingleService.UploadImages(formfile);
                }
            }

            return (errorMessge, imageName);
        }


        private async Task<PaymentIntent> CreatePaymentIntent(ReservationsOrder order)
        {
            StripeConfiguration.ApiKey = _configuration["StripeSettings:SecretKey"];
            var service = new PaymentIntentService();
            var intent = new PaymentIntent();

            //สร้างรายการใหม่
            if (string.IsNullOrEmpty(order.PaymentIntentId))
            {
                var options = new PaymentIntentCreateOptions
                {
                    Amount = (long)order.TotalPrice * 100, // ยอดเงินเท่าไร
                    Currency = "THB", // สกุลเงิน 
                    PaymentMethodTypes = new List<string> { "card" } // วิธีการจ่าย
                };
                intent = await service.CreateAsync(options); // รหัสใบส่งของ
            };

            return intent; // ส่งใบส่งของออกไป
        }


        [HttpPost("CreateOrderByStripe")]
        public async Task<ActionResult> CreateOrderByStripe([FromForm] OrderDto dto)
        {
            var checkuser = await _dataContext.Users.SingleOrDefaultAsync(x => x.Id == dto.UserId);

            if (checkuser == null)
            {
                return HandleResult(Result<string>.Failure("Not Found User"));
            }
            var cart = await RetrieveCart(dto.UserId);
            if (cart == null)
            {
                return HandleResult(Result<string>.Failure("Cart not Found"));
            }



            var order = new ReservationsOrder
            {
                UserId = dto.UserId,
                OrderDate = DateTime.Now,
                OrderStatus = Models.OrderStatus.PendingApproval,
            };



            foreach (var item in cart.Items)
            {

                var locationtest = await _dataContext.Locations.Include(x => x.Category).FirstOrDefaultAsync(x => x.Id == item.Locations.Id);

                if (locationtest == null || locationtest.Category == null)
                {
                    return HandleResult(Result<string>.Failure("Location or Category Not Found"));
                }

                //     var overlappingReservations = await _dataContext.ReservationsOrderItems
                //.Where(x => x.LocationId == item.Locations.Id &&
                //            x.StatusFinished == 1 &&
                //            ((item.StartTime >= x.StartTime && item.StartTime < x.EndTime) ||
                //             (item.EndTime > x.StartTime && item.EndTime <= x.EndTime)))
                //.ToListAsync();

                //     if (overlappingReservations.Any())
                //     {
                //         return HandleResult(Result<string>.Failure("The selected time is already booked."));
                //     }



                var checkReservation = await _dataContext.ReservationsOrderItems
                      .Where(x => x.LocationId == item.Locations.Id && x.StatusFinished == 1 &&
                    ((item.StartTime >= x.StartTime && item.StartTime < x.EndTime) ||
                     (item.EndTime > x.StartTime && item.EndTime <= x.EndTime) ||
                     (x.StartTime >= item.StartTime && x.StartTime < item.EndTime) ||
                     (x.EndTime > item.StartTime && x.EndTime <= item.EndTime)))
                      .ToListAsync();


                if (checkReservation.Any())
                {
                    return HandleResult(Result<string>.Failure("The selected time is already booked."));
                }




                TimeSpan totalHours = item.EndTime - item.StartTime;
                double totalHoursValue = totalHours.TotalHours;
                int totalRoundedHours = (int)Math.Round(totalHoursValue);

                var price = totalRoundedHours * locationtest.Category.Servicefees;

                var orderItem = new ReservationsOrderItem
                {
                    LocationId = item.Locations.Id,
                    Price = price,
                    ReservationsOrder = order,
                    StartTime = item.StartTime,
                    EndTime = item.EndTime,
                    StatusFinished = 1,
                    Objectives = item.Objectives,
                    CountPeople = item.CountPeople,
                };
                order.OrderItems.Add(orderItem);
            }
            order.TotalPrice = order.GetTotalAmount();
            _dataContext.ReservationsOrders.Add(order);

            _dataContext.Carts.Remove(cart);
            _dataContext.CartItems.RemoveRange(cart.Items);
            await _dataContext.SaveChangesAsync();

            if (dto.PaymentMethod == DTOs.OrderDtos.PaymentMethod.CreditCard)
            {
                var intent = await CreatePaymentIntent(order);
                if (!string.IsNullOrEmpty(intent.Id))
                {
                    order.PaymentIntentId = intent.Id; // เอาใบส่งของใส่ในใบสั่งซื้อ
                    order.ClientSecret = intent.ClientSecret; // เอารหัสลับใส่ในใบสั่งซื้อ
                };
            }
            else
            {
                (string errorMessgeMain, string imageNames) =
                await UploadImageMainAsync(dto.OrderImage);
                order.OrderImage = imageNames;
            }
            await _dataContext.SaveChangesAsync();

            var orderItemsHtml = "";

            foreach (var item in order.OrderItems)
            {
                orderItemsHtml += $@"
                    <div style=""border: 1px solid #ccc; padding: 10px; margin: 10px;"">
                        <p><strong>Location:</strong> {item.Location}</p>
                        <p><strong>StartTime:</strong> {item.StartTime}</p>
                        <p><strong>EndTime:</strong> {item.EndTime}</p>
                        <p><strong>Price:</strong> {item.Price}</p>
                    </div>";
            }


    //        var from = new EmailAddress("64123250113@kru.ac.th", "Golf");
    //        var to = new EmailAddress(checkuser.Email);
    //        var subject = "Order Confirmation";
    //        var htmlContent = $@"
    //<div style=""width: 100%; max-width: 600px; margin: 0 auto; font-family: Arial, sans-serif; border: 1px solid #ccc; padding: 20px;"">
    //    <div style=""text-align: center;"">
    //        <img src=""https://api.freelogodesign.org/assets/thumb/logo/a17b07eb64d341ffb1e09392aa3a1698_400.png"" alt=""Company Logo"" style=""max-width: 150px; margin-bottom: 20px;"">
    //        <h2 style=""font-size: 24px; color: #333; margin-bottom: 10px;"">Order Receipt</h2>
    //        <p style=""font-size: 16px; color: #666;"">Thank you for shopping with us!</p>
    //    </div>

    //    <hr style=""border: 1px solid #ccc; margin: 20px 0;"">

    //    <div style=""margin-bottom: 20px;"">
    //        <h3 style=""font-size: 20px; color: #333; margin-bottom: 10px;"">Order Details</h3>
    //        <p><strong>Order ID:</strong> {order.Id}</p>
    //        <p><strong>Order Date:</strong> {order.OrderDate}</p>
    //    </div>

    //    <div style=""margin-bottom: 20px;"">
    //        <h3 style=""font-size: 20px; color: #333; margin-bottom: 10px;"">Personal Information</h3>
    //        <p>{checkuser.FirstName}, {checkuser.LastName}, {checkuser.PhoneNumber}</p>
    //    </div>

    //    <div style=""margin-bottom: 20px;"">
    //        <h3 style=""font-size: 20px; color: #333; margin-bottom: 10px;"">Order Items</h3>
    //        {orderItemsHtml}
    //    </div>

    //    <hr style=""border: 1px solid #ccc; margin: 20px 0;"">

    //    <div style=""text-align: right;"">
    //        <p style=""font-size: 18px; color: #333;""><strong>Total Amount:</strong> {order.TotalPrice}</p>
    //    </div>

    //    <div style=""text-align: center; margin-top: 20px;"">
    //        <p style=""font-size: 16px; color: #666;"">Thank you for your purchase!</p>
    //      </div>
    //        </div>";


    //        var emailMessage = MailHelper.CreateSingleEmail(from, to, subject, htmlContent, htmlContent);
    //        await _sendGridClient.SendEmailAsync(emailMessage);


            return Ok(order);
        }


        //[HttpGet("GetReservationOrderByReservationOrderId")]
        //public async Task<ActionResult> GetReservationOrderByReservationOrderId(int ReservationOrderId)
        //{
        //    var search = await _dataContext.ReservationsOrders.FirstOrDefaultAsync(x=>x.Id == ReservationOrderId);
        //    if(search == null)
        //    {
        //        return HandleResult(Result<string>.Failure("Not Found ReservationOrder"));
        //    }
        //    return Ok(search);
        //}


        //[HttpGet]
        //public async Task<ActionResult>TestOrderById(int OrderId)
        //{
        //    var order = await _dataContext.ReservationsOrders.Include(x=>x.OrderItems).FirstOrDefaultAsync(x=>x.Id.Equals(OrderId));

        //    return Ok(new
        //    {
        //        order = new
        //        {
        //            order.Id,
        //        },
        //        orderItem = order.OrderItems.toList(),
        //        user = await _dataContext.Users.FirstOrDefaultAsync(x=>x.Id.Equals(order.UserId)),
        //    }); ;
        //}

        //[HttpGet]
        //public async Task<ActionResult> TestOrderById(int OrderId)
        //{
        //    var order = await _dataContext.ReservationsOrders.Include(x => x.OrderItems).FirstOrDefaultAsync(x => x.Id.Equals(OrderId));
        //    var user = await _dataContext.Users.FirstOrDefaultAsync(x => x.Id.Equals(order.UserId));

        //    return Ok(new
        //    {
        //        order = new
        //        {
        //            order.Id,
        //        },
        //        orderItem = order.OrderItems.Select(x => new
        //        {
        //            x.StartTime,
        //            x.EndTime
        //        }),
        //        user = new
        //        {
        //            user.FirstName,
        //            user.LastName
        //        },
        //    }); ;
        //}

        [HttpGet("GetReservationOrderByReservationOrderId")]
        public async Task<ActionResult>GetReservationOrderByReservationOrderId(int ReservationOrderId)
        {
            var order = await _dataContext.ReservationsOrders.FirstOrDefaultAsync(x => x.Id == ReservationOrderId);
            var user = await _dataContext.Users.FirstOrDefaultAsync(x => x.Id == order.UserId);
            var test = await _dataContext.Agencys.FirstOrDefaultAsync(x => x.Id == user.AgencyId);

            if (order == null)
            {
                return HandleResult(Result<string>.Failure("Not Found ReservationOrder"));
            }

            return Ok(new
            {
                order = new
                {
                    order.Id,
                },
                user = new
                {
                    user.FirstName,
                    user.LastName,
                    user.PhoneNumber,
                    user.Email,
                },
                test = new
                {
                    test.Name
                }

            }); ;
        }

        [HttpGet("GetOrderByUserId")]
        public async Task<ActionResult> GetOrderByUserId(string UserId)
        {
            var user = await _dataContext.Users.FirstOrDefaultAsync(x => x.Id == UserId);

            if (user == null)
            {
                return HandleResult(Result<string>.Failure("Not Found User"));
            }

            var orders = await _dataContext.ReservationsOrders
                .Include(x => x.OrderItems).ThenInclude(x=>x.Location).ThenInclude(x=>x.Category)
                .Where(x => x.UserId == UserId)
                .ToListAsync();

            var result = new
            {
                user = new
                {
                    user.Email,
                    user.UserName,
                    user.PhoneNumber,
                    user.FirstName,
                    user.LastName,
                    user.AgencyId,
                },
                orders = orders.Select(order => new
                {
                    order.OrderImage,
                    order.OrderDate,
                    order.OrderStatus,
                    order.TotalPrice,
                    orderItems = order.OrderItems.ToList()
                }).ToList()
            };

            return Ok(result);
        }


        //[HttpGet("GetOrderByUserId")]
        //public async Task<ActionResult> GetOrderByUserId(string UserId)
        //{
        //    var user = await _dataContext.Users.SingleOrDefaultAsync(u => u.Id == UserId);

        //    if (user == null)
        //        return HandleResult(Result<string>.Failure("User Not Found"));

        //    var result = await _dataContext.ReservationsOrders.ToListAsync();

        //    if (result == null || result.Count == 0)
        //    {
        //        return HandleResult(Result<string>.Failure("Notfound Order"));
        //    }

        //    return HandleResult(Result<object>.Success(result));
        //}
    }
}
