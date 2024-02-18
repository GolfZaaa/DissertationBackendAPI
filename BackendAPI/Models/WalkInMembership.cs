using System.Text.Json.Serialization;

namespace BackendAPI.Models
{
    public class WalkInMembership
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public DateTime ExpirationTime { get; set; }
        public int LocationId { get; set; }
        [JsonIgnore]
        public Location Location { get; set; }
        public string CreateBy { get; set; }

    }
}
