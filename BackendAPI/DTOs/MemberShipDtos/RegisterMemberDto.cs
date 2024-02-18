namespace BackendAPI.DTOs.MemberShipDtos
{
    public class RegisterMemberDto
    {
        public string userId { get; set; }
        public int LocationId { get; set; }
        public int durationMonths { get; set; }
        public string CreateBy { get; set; }

    }
}
