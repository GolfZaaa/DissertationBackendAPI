﻿namespace BackendAPI.DTOs.MemberShipDtos
{
    public class CreateMembershipPriceDto
    {
        public int LocationId { get; set; }
        public int PriceForMonth { get; set; }
        public int PriceWalkin { get; set; }
    }
}