namespace BackendAPI.DTOs
{
    public class UserDto
    {
        public string UserId { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
        public string username { get; set; }
        public IList<string> role { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string phoneNumber { get; set; }
        public int AgencyId { get; set; }

        public string ProfileImage { get; set; }
    }
}
