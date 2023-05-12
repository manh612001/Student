using StudentClass.Models;

namespace StudentClass.ViewModels
{
    public class ListClassStudentViewModel
    {
        public int Id { get; set; }
        public string ClassName { get; set; }
        public List<Student> StudentName { get; set; }

    }
}
