using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentClass.Application.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        public string? UserName { get; set; }
        [MinLength(6)]
        public string? Password { get; set; }
        public string? ReturnUrl { get; set; }
    }
}
