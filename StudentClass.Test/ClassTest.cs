using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using StudentClass.Interfaces;
using StudentClass.Models;
using StudentClass.Service;
using StudentClass.ViewModels;

namespace StudentClass.Test
{
    [TestFixture]
    public class ClassTest
    {
        private DatabaseDbContext _context;
        private ClassService _classService;
        private AddClassViewModel _addClass;
        private List<AddClassViewModel> _listClass;
        private DbContextOptions<DatabaseDbContext> _options;

        [SetUp]
        public void SetUp()
        {
            _options = new DbContextOptionsBuilder<DatabaseDbContext>()
                .UseInMemoryDatabase("TestDatabase")
                .Options;
            _context = new DatabaseDbContext(_options);
            _classService = new ClassService(_context);
            _addClass = new AddClassViewModel()
            {
                Id = 1,
                Name = "Class 1",
                StudentIds = new List<AddStudentViewModel> {
                    new AddStudentViewModel { Id =1,Name="Student 1",Address="Hà Nội",PhoneNumber="0123456789",Dob= new DateTime(2015, 12, 31, 5, 10, 20)},
                    new AddStudentViewModel { Id =2,Name="Student 2",Address="Hà Nội",PhoneNumber="0123456788",Dob= new DateTime(2015, 12, 31, 5, 10, 20)},
                }
            };
            _listClass = new List<AddClassViewModel>
            {
                new AddClassViewModel
                {  
                    Id = 1,
                    Name = "Class 1",
                    StudentIds = new List<AddStudentViewModel> 
                    {
                        new AddStudentViewModel { Id =1,Name="Student 1",Address="Hà Nội",PhoneNumber="0123456789",Dob= new DateTime(2015, 12, 31, 5, 10, 20)},
                        new AddStudentViewModel { Id =2,Name="Student 2",Address="Hà Nội",PhoneNumber="0123456788",Dob= new DateTime(2015, 12, 31, 5, 10, 20)},
                    }
                },
                new AddClassViewModel
                {  
                    Id = 2,
                    Name = "Class 2",
                    StudentIds = new List<AddStudentViewModel>
                    {
                        new AddStudentViewModel { Id =1,Name="Student 1",Address="Hà Nội",PhoneNumber="0123456789",Dob= new DateTime(2015, 12, 31, 5, 10, 20)},
                        new AddStudentViewModel { Id =2,Name="Student 2",Address="Hà Nội",PhoneNumber="0123456788",Dob= new DateTime(2015, 12, 31, 5, 10, 20)},
                    } 
                }
            };
        }
        [Test]
        public async Task AddClass_Test()
        {
            await _classService.Add(_addClass);
            var result = await _context.classes.FindAsync(_addClass.Id);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Name, Is.EqualTo(_addClass.Name));

            Assert.That(result.StudentInClasses.Count == 2, Is.True);
        }
        [Test]
        public async Task GetClassById_Test()
        {

            await _classService.Add(_addClass);


            var result = await _classService.GetById(_addClass.Id);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(_addClass.Id));
            Assert.That(result.Name, Is.EqualTo(_addClass.Name));
            Assert.That(result.StudentIds.Count == 2, Is.True);
        }
        [Test]
        public async Task UpdateClass_Test()
        {
            await _classService.Add(_addClass);
            // Act
            _addClass.Name = "Test";
            var existingStudent = await _context.classes.FindAsync(_addClass.Id);
            if (existingStudent != null)
            {
                _context.Entry(existingStudent).State = EntityState.Detached;
            }
            await _classService.Update(_addClass);

            // Assert
            var result = await _context.classes.FindAsync(_addClass.Id);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Name, Is.EqualTo(_addClass.Name));
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
            Assert.That(result[0].ClassName, Is.EqualTo(_listClass[0].Name));
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
