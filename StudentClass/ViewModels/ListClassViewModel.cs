using StudentClass.Models;

namespace StudentClass.ViewModels
{
    public class ListClassViewModel
    {
        public int Id { get; set; }
        public string ClassName { get; set; }
        public List<Student> Students { get; set; }
    }
}
