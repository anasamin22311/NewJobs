using Jobs.Models;

namespace Jobs.ViewModels
{
    public class JobViewModel
    {
        public string Title  { get; set; }
        public ICollection<ApplytForJob> JobApplications { get; set; }
    }
}
