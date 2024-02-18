using System.Text.Json.Serialization;

namespace BackendAPI.Models
{
    public class WalkInTransaction
    {
        public int Id { get; set; }
        public DateTime TransactionDate { get; set; }
        public int NumberOfPeople { get; set; }
        public int LocationId { get; set; }
        //public int WalkInMembershipId { get; set; }
        //[JsonIgnore]
        //public WalkInMembership WalkInMembership { get; set; }
        public string UserId { get; set; } = "0";
        public string CreateBy { get; set; }
    }
}
