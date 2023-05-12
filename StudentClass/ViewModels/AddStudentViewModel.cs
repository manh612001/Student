﻿using System.ComponentModel.DataAnnotations;

namespace StudentClass.ViewModels
{
    public class AddStudentViewModel
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public DateTime Dob { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public string Address { get; set; }

    }
}
