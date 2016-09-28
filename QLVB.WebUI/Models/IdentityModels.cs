using Microsoft.AspNet.Identity.EntityFramework;

namespace QLVB.WebUI.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public int UserId { get; set; }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection")
        {
        }
    }

    public class UserOwin
    {
        public string Identity { get; set; }
        public string UserName { get; set; }
        public int UserId { get; set; }
    }
}