using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using StudentClass.Interfaces;
using StudentClass.Models;
using StudentClass.ViewModels;

namespace StudentClass.Service
{
    public class ClassService : IClassService
    {
        private readonly DatabaseDbContext _context;
        public ClassService(DatabaseDbContext context)
        {
            _context = context;
        }

        public async Task<AddClassViewModel> Add(AddClassViewModel viewModel)
        {
            
            try
            {
                var newClass = new Class()
                {
                    Name = viewModel.Name,
                };
                _context.classes.Add(newClass);
                await _context.SaveChangesAsync();
                
                foreach (var item in viewModel.StudentIds)
                {
                    _context.studentInClass.Add(new StudentInClass()
                    {
                        ClassId = newClass.Id,
                        StudentId = item.Id
                    });
                }
                await _context.SaveChangesAsync();

                return viewModel;
            }
            catch (Exception)
            {
                throw new Exception("Error can't add Class");
            }


        }

        public async Task<bool> Delete(int? id)
        {
            try
            {
                var classes = await _context.classes.FindAsync(id);
                if (classes == null)
                {
                    throw new Exception("Not Found Class");
                }
                _context.classes.Remove(classes);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {

                throw new Exception("Error can't delete class");
            }           

        }

        public async Task<ListClassViewModel> Get(int? id)
        {
            var classes = await _context.classes.Include(c => c.StudentInClasses)
                                                    .ThenInclude(cs => cs.Student)
                                                    .FirstOrDefaultAsync(c => c.Id == id);
            if (classes == null)
            {
                return new ListClassViewModel();
            }
            ListClassViewModel vm = new ListClassViewModel()
            {
                Id = classes.Id,
                ClassName = classes.Name,
                Students = classes.StudentInClasses.Select(sc => new Student
                {
                    Id = sc.StudentId,
                    Name = sc.Student.Name
                }).ToList()
            };
            return vm;
        }

        public async Task<List<ListClassStudentViewModel>> GetAll()
        {
            var classes = await _context.classes
                                        .Include(c => c.StudentInClasses)
                                            .ThenInclude(cs => cs.Student)
                                        .Select(c => new ListClassStudentViewModel
                                        {
                                            Id = c.Id,
                                            ClassName = c.Name,
                                            StudentName = c.StudentInClasses
                                            .Select(sc => new Student
                                            {
                                                Id = sc.StudentId,
                                                Name = sc.Student.Name
                                            }).ToList()
                                        }).ToListAsync();
            return classes;
        }

        public async Task<AddClassViewModel> GetById(int? id)
        {

            var classes = await _context.classes.Include(c => c.StudentInClasses)
                                                    .ThenInclude(cs => cs.Student)
                                                    .FirstOrDefaultAsync(c => c.Id == id);
            if (classes == null)
            {
                return new AddClassViewModel();
            }
            AddClassViewModel vm = new AddClassViewModel()
            {
                Id = classes.Id,
                Name = classes.Name,
                StudentIds = classes.StudentInClasses
                                    .Select(sc => new AddStudentViewModel 
                                    { 
                                        Id = sc.StudentId,
                                        Name = sc.Student.Name
                                    })
                                    .ToList()
            };
            return vm;

            
        }

        public async Task<AddClassViewModel> Update(AddClassViewModel viewmodel)
        {
           
            var classResult = await _context.classes.FindAsync(viewmodel.Id);

            if (classResult == null)
            {
                throw new Exception("Not Found");
            }
            try
            {
                classResult.Name = viewmodel.Name;

                List<AddStudentViewModel> oldStudent = await _context.studentInClass.Where(x => x.ClassId == viewmodel.Id)
                                                .Select(x => new AddStudentViewModel { Id = x.StudentId})
                                               .ToListAsync();
                 

                List<AddStudentViewModel> deleteStudent = oldStudent.Except(viewmodel.StudentIds).ToList();

                foreach (var item in deleteStudent)
                {
                    StudentInClass studentInClass = await _context.studentInClass
                                                                  .Where(x => x.ClassId == viewmodel.Id && x.StudentId == item.Id)
                                                                  .FirstOrDefaultAsync();
                    _context.studentInClass.Remove(studentInClass);
                }

                List<AddStudentViewModel> addStudent = viewmodel.StudentIds.Except(oldStudent).ToList();

                foreach (var item in addStudent)
                {
                   
                    _context.studentInClass.Add(new StudentInClass()
                    {
                        ClassId = viewmodel.Id,
                        StudentId = item.Id
                    });
                }
                await _context.SaveChangesAsync();
                return viewmodel;
            }
            catch (Exception)
            {
                throw new Exception("Error can't update class");
            }


            
        }
    }
}
