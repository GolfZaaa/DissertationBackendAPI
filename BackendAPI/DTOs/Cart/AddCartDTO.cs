namespace BackendAPI.DTOs.Cart
{
    public class AddCartDTO
    {
        public int LocationId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int CountPeople { get; set; }
        public string userId { get; set; }

    }
}
