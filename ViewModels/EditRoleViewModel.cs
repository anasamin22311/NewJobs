using System.ComponentModel.DataAnnotations;

namespace Jobs.ViewModels
{
    public class EditRoleViewModel
    {
        public string Id { get; set; }
        [Required (ErrorMessage ="هذا الحقل مطلوب")]
        public string Name { get; set; }
        public List<string>Users { get; set; }

    }
}
