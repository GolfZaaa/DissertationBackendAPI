using Microsoft.AspNetCore.Identity;

namespace BackendAPI.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string ProfileImage { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public int AgencyId { get; set; } = 0;
        public int StatusOnOff { get; set; } = 1;
    }
}
