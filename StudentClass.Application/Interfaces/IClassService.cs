using StudentClass.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentClass.Application.Interfaces
{
    public interface IClassService
    {
        Task Add(ClassViewModel.CreateClass model);
        Task Update(ClassViewModel.ClassDetail model);
        Task Delete(int id);
        Task<ClassViewModel.ClassDetail> GetById(int id);
        Task<List<ClassViewModel.CreateClass>> GetAll();
    }
}
