using StudentClass.ViewModels;

namespace StudentClass.Interfaces
{
    public interface IClassService
    {
        Task<List<ListClassStudentViewModel>> GetAll();
        Task<AddClassViewModel> GetById(int? id);
        Task<ListClassViewModel> Get(int? id);
        Task<AddClassViewModel> Add(AddClassViewModel viewModel);
        Task<AddClassViewModel> Update(AddClassViewModel viewmodel);
        Task<bool> Delete(int? id);
        
    }
}
