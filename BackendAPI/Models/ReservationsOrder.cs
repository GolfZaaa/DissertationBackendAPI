using Stripe;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BackendAPI.Models;
public class ReservationsOrder
{
    //public int Id { get; set; }
    //public DateTime DateTimeCreateReservations { get; set; }
    //public DateTime StartTime { get; set; }
    //public DateTime EndTime { get; set; }
    //public int CountPeople { get; set; }
    //public int Price { get; set; }
    //public ApplicationUser Users { get; set; }
    //public Location Locations { get; set; }
    //public int StatusFinished { get; set; }
    [Key]
    public int Id { get; set; }
    public string? OrderImage { get; set; }
    public DateTime OrderDate { get; set; }

    public long TotalAmount { get; set; }

    [JsonIgnore]
    public List<ReservationsOrderItem> OrderItems { get; set; } = new List<ReservationsOrderItem>();
    public int StatusFinished { get; set; }

}
