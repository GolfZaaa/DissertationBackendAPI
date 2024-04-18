namespace BackendAPI.DTOs.MemberShipDtos
{
    public class UpdateMembershipPriceDto
    {
        public int Id { get; set; }
        public int LocationId { get; set; }
        public int PriceForMonth { get; set; }
        public int PriceWalkin {  get; set; }
    }
}
