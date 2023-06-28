using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StudentClass.Application.Interfaces;
using StudentClass.Application.ViewModels;
using StudentClass.Domain;
using StudentClass.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentClass.Infrastructure.Services
{
    public class ClassService : IClassService
    {
        private readonly DatabaseDbContext _context;
        
        public ClassService(DatabaseDbContext context)
        {
            _context = context;
        }
        public async Task Add(ClassViewModel.CreateClass model)
        {
            var newClass = new StudentClass.Domain.Class()
            {
                Name = model.Name,
            };
            _context.Class.Add(newClass);
            await _context.SaveChangesAsync();
            if (model.StudentIds != null)
            {
                foreach (var studentId in model.StudentIds)
                {
                    _context.StudentInClass.Add(new StudentInClass()
                    {
                        ClassId = newClass.Id,
                        StudentId = studentId
                    });
                }
                await _context.SaveChangesAsync();
            }
        }

        public async Task Delete(int id)
        {
            var result = await _context.Class.FindAsync(id);
            if (result == null)
            {
                throw new ArgumentNullException("Class not found!");
            }
            _context.Class.Remove(result);
            await _context.SaveChangesAsync();
        }

        public async Task<List<ClassViewModel.CreateClass>> GetAll()
        {
            return await _context.Class.Select(c => new ClassViewModel.CreateClass()
            {
                Id = c.Id,
                Name = c.Name,
                Students = c.StudentInClasses.Select(x => new StudentViewModel.Student()
                {
                    Id = x.StudentId,
                    Name = x.Student.Name
                }).ToList(),
            }).ToListAsync(); ;
        }

        public async Task<ClassViewModel.ClassDetail> GetById(int id)
        {
            var query = await _context.Class.FindAsync(id);
            if(query ==null)
            {
                throw new ArgumentNullException("Class not found!");
            }
            var result = new ClassViewModel.ClassDetail()
            {
                Id = query.Id,
                Name = query.Name,
                Students = await _context.Student.ToListAsync(),
                StudentIds = _context.StudentInClass.Where(sc => sc.ClassId == id).Select(sc => sc.StudentId).ToList()
            };
            return result; 
        }

        public async Task Update(ClassViewModel.ClassDetail model)
        {
            var classResult = await _context.Class.FindAsync(model.Id);
            if(classResult == null) 
            {
                throw new ArgumentNullException("Class not found");
            }
            classResult.Name = model.Name;
            _context.Class.Update(classResult);
            await _context.SaveChangesAsync();

            var result = await _context.StudentInClass.Where(sc => sc.ClassId == model.Id).ToListAsync();
            _context.StudentInClass.RemoveRange(result);
            await _context.SaveChangesAsync();
            if(model.StudentIds != null)
            {
                foreach (var studentId in model.StudentIds)
                {
                    _context.StudentInClass.Add(new StudentInClass()
                    {
                        ClassId = model.Id,
                        StudentId = studentId
                    });
                }
                await _context.SaveChangesAsync();
            }
        }
    }
}
