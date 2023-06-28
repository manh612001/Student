using Microsoft.AspNetCore.Identity;

namespace StudentClass.Domain
{
    public class AppUser:IdentityUser
    {
        public string? Name { get; set; }
        public string? Role { get; set; }
    }
}
