using System.ComponentModel.DataAnnotations;

namespace Jobs.Models
{
    public class Category
    {
        public int Id { get; set; }
        [Required]
        [Display(Name="نوع الوظيفة")]
        public string Name { get; set; }
        [Required]
        [Display(Name="وصف النوع")]
        public string Description { get; set; }
        public ICollection<Job>? Jobs { get; set; }
    }
}
