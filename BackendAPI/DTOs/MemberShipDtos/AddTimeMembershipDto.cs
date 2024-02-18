namespace BackendAPI.DTOs.MemberShipDtos
{
    public class AddTimeMembershipDto
    {
        public string userId { get; set; }
        public int locationId { get; set; }
        public int months { get; set; }
    }
}
