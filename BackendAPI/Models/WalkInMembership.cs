using System.Text.Json.Serialization;

namespace BackendAPI.Models
{
    public class WalkInMembership
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public DateTime StartTime { get; set; } 
        public DateTime EndTime { get; set; }
        public int MembershipPriceId { get; set; }
        [JsonIgnore]
        public MembershipPrice MembershipPrice { get; set; }
        public string CreateBy { get; set; }
    }
}
