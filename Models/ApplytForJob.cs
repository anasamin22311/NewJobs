using Microsoft.AspNetCore.Identity;

namespace Jobs.Models
{
    public class ApplytForJob
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public DateTime ApplyDate { get; set; }

        public string UserId { get; set; }
        public virtual ApplicationUser user { get; set; }

        public int JobId { get; set; }
        public virtual Job job { get; set; }
    }
}
