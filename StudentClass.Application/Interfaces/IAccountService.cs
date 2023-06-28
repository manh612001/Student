using StudentClass.Application.ViewModels;
using StudentClass.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentClass.Application.Interfaces
{
    public interface IAccountService
    {
        Task<string> LoginAsync(LoginViewModel login);
        Task LogoutAsync();
        Task SignUp(SignUpViewModel signUp);
        Task<AppUserViewModel> GetByName(string name);
        Task<bool> ExternalLogin();
        Task<string> LoginAPI(LoginViewModel login);
    }
}
