using Microsoft.AspNetCore.Identity;

namespace Jobs.Models
{
    public class ApplicationUser : IdentityUser
    {
        public virtual ICollection<Job> Jobs { get; set; }
    }
}
