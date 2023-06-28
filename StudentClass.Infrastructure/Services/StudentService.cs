using Microsoft.EntityFrameworkCore;
using StudentClass.Application.Interfaces;
using StudentClass.Application.ViewModels;
using StudentClass.Domain;
using StudentClass.Infrastructure.Data;
namespace StudentClass.Infrastructure.Services
{
    public class StudentService : IStudentService
    {
        private readonly DatabaseDbContext _context;

        public StudentService(DatabaseDbContext context)
        {
            _context = context;
        }
        public async Task Add(StudentViewModel.Student model)
        {
            var obj = new Student()
            {
                Name = model.Name,
                Address = model.Address,
                Dob = model.Dob,
                PhoneNumber = model.PhoneNumber,
            };
            _context.Student.Add(obj);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var student = await _context.Student.FindAsync(id);
            if (student == null)
            {
                throw new ArgumentNullException("Student not found");
            }
            _context.Student.Remove(student);
            await _context.SaveChangesAsync();
        }

        public async Task<List<StudentViewModel.Student>> GetAll() // get studentselectlist
        {
            return await _context.Student.Select(x => new StudentViewModel.Student
            {
                Id = x.Id,
                Name = x.Name,
            }).ToListAsync();

        }

        public async Task<StudentViewModel.Student> GetById(int id)
        {
            var student = await _context.Student.FindAsync(id);
            if (student == null)
            {
                throw new ArgumentNullException("Student not found");
            }
            return new StudentViewModel.Student()
            {
                Id = student.Id,
                Name = student.Name,
                PhoneNumber = student.PhoneNumber,
                Address = student.Address,
                Dob = student.Dob,
            };
        }

        public async Task<List<StudentViewModel.StudentWithClass>> StudentWithClass()
        {
            var result = await _context.Student.Select(x => new StudentViewModel.StudentWithClass
            {
                Id = x.Id,
                Name = x.Name,
                Class = x.StudentInClasses!.Select(sc => new ClassViewModel.Class()
                {
                    Id = sc.ClassId,
                    Name = sc.Class!.Name
                }).ToList()
            }).ToListAsync();

            return result;
        }

        public async Task Update(StudentViewModel.Student model)
        {
            var obj = new Student()
            {
                Id = model.Id,
                Name = model.Name,
                Address = model.Address,
                Dob = model.Dob,
                PhoneNumber = model.PhoneNumber,
            };
            _context.Student.Update(obj);
            await _context.SaveChangesAsync();
        }
    }
}
