using System.Text.Json.Serialization;

namespace BackendAPI.Models
{
    public class MembershipPrice
    {
        public int Id { get; set; }
        public int PriceForMonth { get; set; }
        public int PriceWalkin { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
        public int LocationId { get; set; }
        [JsonIgnore]
        public Location Location { get; set; }
    }
}
