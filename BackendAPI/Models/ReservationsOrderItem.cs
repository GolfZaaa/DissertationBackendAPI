using Stripe.Climate;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BackendAPI.Models
{
    public class ReservationsOrderItem
    {
        [Key]
        public int Id { get; set; }
        public int LocationId { get; set; }
        public Location Location { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Objectives { get; set; }
        public int CountPeople { get; set; }
        public long Price { get; set; }
        public int ReservationsOrderId { get; set; }
        [JsonIgnore]
        public ReservationsOrder ReservationsOrder { get; set; }
        public int StatusFinished { get; set; }

    }
}
