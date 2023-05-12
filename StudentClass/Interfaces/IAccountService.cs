using StudentClass.Models;
using StudentClass.ViewModels;

namespace StudentClass.Interfaces
{
    public interface IAccountService
    {
        Task LoginAsync(LoginViewModel login);
        Task LogoutAsync();
        
        Task Registration(RegistrationViewModel registration);
        Task<string> LoginAPI(LoginViewModel login);
    }
}
