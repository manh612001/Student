using System.ComponentModel.DataAnnotations;

namespace StudentClass.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        public string UserName { get; set; }
        [MinLength(6)]
        public string Password { get; set; }
    }
}
