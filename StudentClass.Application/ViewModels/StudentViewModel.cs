using StudentClass.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentClass.Application.ViewModels
{
    public class StudentViewModel
    {
        public class Student
        {
            public int Id { get; set; }
            [Required]
            public string? Name { get; set; }
            [Required]
            public DateTime Dob { get; set; }
            [Required]
            public string? PhoneNumber { get; set; }
            [Required]
            public string? Address { get; set; }
            
        }
        public class StudentWithClass
        {
            public int Id { get; set; }
            public string? Name { get; set; }
            public List<ClassViewModel.Class>? Class { get; set; }
        }
    }
}
