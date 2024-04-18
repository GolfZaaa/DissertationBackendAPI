using BackendAPI.DTOs.OrderDtos;
using Stripe;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BackendAPI.Models;
public class ReservationsOrder
{
    [Key]
    public int Id { get; set; }
    public string UserId { get; set; }
    public string? OrderImage { get; set; }
    public DateTime OrderDate { get; set; }

    [JsonIgnore]
    public List<ReservationsOrderItem> OrderItems { get; set; } = new List<ReservationsOrderItem>();
    public string? PaymentIntentId { get; set; }
    public string? ClientSecret { get; set; }
    public OrderStatus OrderStatus { get; set; }

    public long GetTotalAmount()
    {
        return OrderItems.Sum(x => x.Price);
    }

}
