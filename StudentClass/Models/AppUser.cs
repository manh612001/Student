using Microsoft.AspNetCore.Identity;

namespace StudentClass.Models
{
    public class AppUser:IdentityUser
    {
        public string Name { get; set; }
    }
}
