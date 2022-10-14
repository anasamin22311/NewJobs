using System.ComponentModel.DataAnnotations;

namespace Jobs.ViewModels
{
    public class CreateRoleViewModel
    {

        public int Id { get; set; }
        [Required]
        public string RoleName { get; set; }
    }
}
