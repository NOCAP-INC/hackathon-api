using Microsoft.AspNetCore.Identity;

namespace NoCap.Managers
{
    public class User : IdentityUser
    {

        public string FullName { get; set; }
        public string RoleId { get; set; }

    }
}
