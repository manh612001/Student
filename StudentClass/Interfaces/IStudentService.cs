
using StudentClass.Models;
using StudentClass.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentClass.Interfaces
{
    public interface IStudentService
    {
        Task<List<Student>> GetAll();
        Task<List<ListStudentViewModel>> StudentWithClass();
        Task<EditStudentViewModel> GetById(int? id);
        Task<Student> GetDetail(int? id);
        Task<AddStudentViewModel> Add(AddStudentViewModel student);
        Task<AddStudentViewModel> Update(AddStudentViewModel student);
       
        Task Delete(int? id);
        
    }
}
