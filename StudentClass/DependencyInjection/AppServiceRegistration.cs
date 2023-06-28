using StudentClass.Application.Interfaces;
using StudentClass.Infrastructure.Services;

namespace StudentClass.MVC.DependencyInjection
{
    public static class AppServiceRegistration
    {
        public static void ConfigureAppService(this IServiceCollection services)
        {
            services.AddScoped<IStudentService, StudentService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IClassService, ClassService>();
        }
    }
}
