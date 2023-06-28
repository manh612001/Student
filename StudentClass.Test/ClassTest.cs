using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using StudentClass.Application.ViewModels;
using StudentClass.Domain;
using StudentClass.Infrastructure.Data;
using StudentClass.Infrastructure.Services;


namespace StudentClass.Test
{
    [TestFixture]
    public class ClassTest
    {
        private DatabaseDbContext _context;
        private ClassService _classService;
        private ClassViewModel.CreateClass _createClass;
        private List<ClassViewModel.CreateClass> _listClass;
        private ClassViewModel.ClassDetail _classDetail;
        private DbContextOptions<DatabaseDbContext> _options;
        [SetUp]
        public void SetUp()
        {
            _options = new DbContextOptionsBuilder<DatabaseDbContext>()
                .UseInMemoryDatabase("TestDatabase")
                .Options;
            _context = new DatabaseDbContext(_options);
            _classService = new ClassService(_context);
            _createClass = new ClassViewModel.CreateClass()
            {
                Id = 1,
                Name = "Class 1",
                Students = new List<StudentViewModel.Student> {
                    new StudentViewModel.Student { Id = 1, Name = "Student 1"},
                    new StudentViewModel.Student { Id = 2, Name = "Student 2"},
                }       
            };
            _listClass = new List<ClassViewModel.CreateClass>
            {
                new ClassViewModel.CreateClass
                {
                    Id = 1,
                    Name = "Class 1",
                    Students = new List<StudentViewModel.Student>
                    {
                        new StudentViewModel.Student {Id = 1, Name = "Student 1"},
                        new StudentViewModel.Student {Id = 2, Name = "Student 2"},
                    }
                },
                new ClassViewModel.CreateClass
                {
                    Id = 2,
                    Name = "Class 2",
                    Students = new List<StudentViewModel.Student>
                    {
                        new StudentViewModel.Student { Id = 1, Name = "Student 1"},
                        new StudentViewModel.Student { Id = 2, Name = "Student 2"}
                    }
                }
            };
            _classDetail = new ClassViewModel.ClassDetail()
            {
                Id = 1,
                Name = "Class 1.1",
                Students = new List<Student>()
                {
                    new Student()
                    {
                        Id = 1,
                        Name = "Student 1",
                        Dob = new DateTime(2023,6,19),
                        Address = "Hà Nội",
                        PhoneNumber = "012345678"
                    },
                    new Student()
                    {
                        Id = 2,
                        Name = "Student 2",
                        Dob = new DateTime(2023,6,19),
                        Address = "Hà Nội",
                        PhoneNumber = "012345678"
                    }

                },
                StudentIds = null
            };
        }
        [Test]
        public async Task AddClass_Test()
        {
            await _classService.Add(_createClass);
            var result = await _context.Class.FindAsync(_createClass.Id);
            //var check = await _context.StudentInClass.Where(x=>x.StudentId == _createClass.Id).ToListAsync();
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Name, Is.EqualTo(_createClass.Name));
            //Assert.That(result.StudentInClasses?.Count == 2, Is.True);
        }
        [Test]
        public async Task GetClassById_Test()
        {
            await _classService.Add(_createClass);
            var result = await _classService.GetById(_createClass.Id);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(_createClass.Id));
            Assert.That(result.Name, Is.EqualTo(_createClass.Name));
            //Assert.That(result.Students?.Count() == 2, Is.True);
        }
        [Test]
        public async Task UpdateClass_Test()
        {
            await _classService.Add(_createClass);
            // Act
            
            var existingStudent = await _context.Class.FindAsync(_createClass.Id);
            if (existingStudent != null)
            {
                _context.Entry(existingStudent).State = EntityState.Detached;
            }
            await _classService.Update(_classDetail) ;

            // Assert
            var result = await _context.Class.FindAsync(_createClass.Id);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Name, Is.EqualTo(_classDetail.Name));
        }
        [Test]
        public async Task GetAll_Test()
        {
            foreach (var item in _listClass)
            {
                await _classService.Add(item);
            }
            var result = await _classService.GetAll();
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count == 2, Is.True);
            Assert.That(result[0].Name, Is.EqualTo(_listClass[0].Name));
        }
        [TearDown]
        public void TearDown()
        {
            using (var context = new DatabaseDbContext(_options))
            {
                context.Database.EnsureDeleted();
            }
        }
    }
}
