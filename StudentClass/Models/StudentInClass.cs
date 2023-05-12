using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentClass.Models
{
    public class StudentInClass
    {
        
        public int StudentId { get; set; }
        public int ClassId { get; set; }
        public Student Student { get; set; }
        public Class Class { get; set; }
    }
}
