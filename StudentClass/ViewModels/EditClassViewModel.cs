using StudentClass.Models;
using System.ComponentModel.DataAnnotations;

namespace StudentClass.ViewModels
{
    public class EditClassViewModel
    {
        public int Id { get; set; }
        [Required]
        public string ClassName { get; set; }
        public List<int> studentIds { get; set; } = new() { };
    }
}
