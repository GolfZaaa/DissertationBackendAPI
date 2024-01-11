using Microsoft.AspNetCore.Identity;

namespace BackendAPI.Models
{
    public class ApplicationUser : IdentityUser
    {
        //public int ProfileImage { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int AgencyId { get; set; }
    }
}
