using Stripe;

namespace BackendAPI.DTOs.OrderDtos
{
    public class OrderDto
    {
        public string UserId { get; set; }
        //public PaymentMethod PaymentMethod { get; set; }
        public IFormFile? OrderImage { get; set; }
        public PaymentMethod PaymentMethod { get; set; }

    }
}
