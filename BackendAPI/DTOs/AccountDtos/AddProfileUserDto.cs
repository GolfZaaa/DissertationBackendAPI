namespace BackendAPI.DTOs.AccountDtos
{
    public class AddProfileUserDto
    {
        public string userId { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string phoneNumber { get; set; }
        public int agencyId { get; set; }
    }
}
