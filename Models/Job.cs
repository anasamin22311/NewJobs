using Microsoft.AspNetCore.Identity;
using Microsoft.Build.Framework;
using System.ComponentModel;
namespace Jobs.Models
{
    public class Job
    {
        public int Id { get; set; }
        
        [DisplayName("إسم الوظيفة")]
        public string Title { get; set; }
        
        [DisplayName("وصف الوظيفة")]
        public string Content { get; set; }
        
        [DisplayName("صورة الوظيفة")]
        public string Image { get; set; }
        
        [DisplayName("نوع الوظيفة")]
        public int CategoryId { get; set; }
        public Category? Category { get; set; }

        public string UserId { get; set; }
        public ApplicationUser? User{ get; set; }

        public virtual ICollection<ApplytForJob> JobApplications { get; set; }
    }
}