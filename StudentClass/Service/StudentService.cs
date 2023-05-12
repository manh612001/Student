
using Microsoft.EntityFrameworkCore;
using StudentClass.Interfaces;
using StudentClass.Models;
using StudentClass.ViewModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace StudentClass.Service
{
    public class StudentService : IStudentService
    {
        private readonly DatabaseDbContext _context;

        public StudentService(DatabaseDbContext context)
        {
            _context = context;

        }

        public async Task<AddStudentViewModel> Add(AddStudentViewModel student)
        {
            
            try
            {
                var obj = new Student()
                {
                    Name = student.Name,
                    Dob = student.Dob,
                    PhoneNumber = student.PhoneNumber,
                    Address = student.Address,
                };

                _context.students.Add(obj);
                await _context.SaveChangesAsync();

                return student;
            }
            catch (Exception e) 
            {

                throw new Exception(e.Message);
            }

        }

        public async Task Delete(int? id)
        {

            var student = await _context.students.FindAsync(id);
            if (student == null)
            {
                throw new Exception("Not Found");
            }
            try
            {
                _context.students.Remove(student);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {

                throw new Exception(e.Message);
            }
        }

        public async Task<List<Student>> GetAll()
        {
            try
            {
                return await _context.students.ToListAsync();
            }
            catch (Exception e)
            {

                throw new Exception(e.Message);
            }
            
        }

        public async Task<EditStudentViewModel> GetById(int? id)
        {
            try
            {
                var student = await _context.students.FindAsync(id);
                if (student != null)
                {
                    var obj = new EditStudentViewModel()
                    {
                        Id = student.Id,
                        Name = student.Name,
                        PhoneNumber = student.PhoneNumber,
                        Dob = student.Dob,
                        Address = student.Address,
                    };
                    return obj;
                }

                return new EditStudentViewModel();
            }
            catch (Exception e)
            {

                throw new Exception(e.Message);
            }
           
        }

        public async Task<Student> GetDetail(int? id)
        {
            try
            {
                var student = await _context.students
                                        .Include(x => x.StudentInClasses)
                                            .ThenInclude(x => x.Class)
                                        .SingleOrDefaultAsync(x => x.Id == id);
                return student;
            }
            catch (Exception e)
            {

                throw new Exception(e.Message);
            }
            
        }

        public async Task<List<ListStudentViewModel>> StudentWithClass()
        {
            var student = await _context.students
                                        .Include(x => x.StudentInClasses)
                                            .ThenInclude(x => x.Class)
                                        .Select(s => new ListStudentViewModel
                                        {
                                            Id = s.Id,
                                            Name = s.Name,
                                            Classes = s.StudentInClasses.Select(sc => new Class
                                            {
                                                Id = sc.ClassId,
                                                Name = sc.Class.Name
                                            }).ToList()
                                        })
                                        .ToListAsync();
            return student;
        }



        public async Task<AddStudentViewModel> Update(AddStudentViewModel student)
        {


            try
            {
                var obj = new Student()
                {
                    Id = student.Id,
                    Name = student.Name,
                    Address = student.Address,
                    Dob = student.Dob,
                    PhoneNumber = student.PhoneNumber,
                };

                _context.students.Update(obj);
                await _context.SaveChangesAsync();
                return student;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

        }



    }



}
