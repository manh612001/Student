using StudentClass.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentClass.Application.Interfaces
{
    public interface IStudentService
    {
        Task Add(StudentViewModel.Student model);
        Task<StudentViewModel.Student> GetById(int id);
        Task<List<StudentViewModel.Student>> GetAll();
        Task Update(StudentViewModel.Student model);
        Task Delete(int id);
        Task<List<StudentViewModel.StudentWithClass>> StudentWithClass();

    }
}
