namespace BackendAPI.DTOs.MemberShipDtos
{
    public class RegisterMemberDto
    {
        public string userId { get; set; }
        public int MembershipPriceId { get; set; }
        public int durationMonths { get; set; }
        //public DateTime startTime { get; set; }
        //public DateTime endTime { get; set; }
        public string CreateBy { get; set; }

    }
}
