using StudentClass.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentClass.Application.ViewModels
{
    public class ClassViewModel
    {
        public class Class
        {
            public int Id { get; set; }
            public string? Name { get; set; }
        }
        public class CreateClass
        {
            public int Id { get; set; }
            [Required]
            public string? Name { get; set; }
            
            public List<StudentViewModel.Student>? Students { get; set; }
            [Required]  
            public List<int>? StudentIds { get; set; }
        }
        public class ClassDetail
        {
            public int Id { get; set; }
            public string? Name { get; set; }
            public List<Student>? Students { get; set; }
            public List<int>? StudentIds { get; set; }
        }
    }
}
