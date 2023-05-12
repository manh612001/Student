using StudentClass.Models;
using System.ComponentModel.DataAnnotations;

namespace StudentClass.ViewModels
{
    public class AddClassViewModel
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public List<AddStudentViewModel> StudentIds { get; set; }
    }
}
