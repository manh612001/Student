using StudentClass.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentClass.ViewModels
{
    public class ListStudentViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Class> Classes { get; set; } = new();
    }
}
