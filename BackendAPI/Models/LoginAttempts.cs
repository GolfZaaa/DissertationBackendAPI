namespace BackendAPI.Models;
    public class LoginAttempts
    {
        public string UserName { get; set; }
        public DateTime DateTimeLogin { get; set; }
        public int CountTimeLogin { get; set; }
    }
