using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using StudentClass.Interfaces;
using StudentClass.Models;
using StudentClass.Service;
using StudentClass.ViewModels;

namespace StudentClass.Test
{
    [TestFixture]
    public class StudentTest
    {
        private IStudentService _studentService;
        private DbContextOptions<DatabaseDbContext> _options;
        private DatabaseDbContext _context;
        private AddStudentViewModel _addStudent;
        private List<AddStudentViewModel> _listStudent;
        [SetUp]
        public void Setup()
        {
            _options = new DbContextOptionsBuilder<DatabaseDbContext>()
                .UseInMemoryDatabase("TestDatabase")
                .Options;
            _context = new DatabaseDbContext(_options);
            _studentService = new StudentService(_context);
            _addStudent = new AddStudentViewModel()
            {
                Id = 1,
                Name = "Test",
                Dob = new DateTime(2023, 1, 1),
                Address = "Hà Nội",
                PhoneNumber = "1234567890",
            };
            _listStudent = new List<AddStudentViewModel>()
            {
                new AddStudentViewModel{ Id = 1,Name="Student 1",Address ="Hà Nội",PhoneNumber ="0123456789"},
                new AddStudentViewModel{ Id = 2,Name="Student 2",Address ="Hà Nội",PhoneNumber ="0123456788"}
            };
        }

        [Test]

        public async Task TestAddStudentAsync()
        {

            await _studentService.Add(_addStudent);

            // Assert
            var result = _context.students.FirstOrDefault(c => c.Id == _addStudent.Id);
            Assert.NotNull(result);
            Assert.That(result.Name, Is.EqualTo(_addStudent.Name));
            Assert.That(result.Dob, Is.EqualTo(_addStudent.Dob));
            Assert.That(result.Address, Is.EqualTo(_addStudent.Address));



        }
        [Test]
        public async Task GetById_Test()
        {
            await _studentService.Add(_addStudent);
            var result = await _studentService.GetById(_addStudent.Id);
            Assert.NotNull(result);
            Assert.That(result.Id, Is.EqualTo(_addStudent.Id));
            Assert.That(result.Name, Is.EqualTo(_addStudent.Name));
            Assert.That(result.Dob, Is.EqualTo(_addStudent.Dob));
            Assert.That(result.Address, Is.EqualTo(_addStudent.Address));
        }
        [Test]
        public async Task GetAll_Test()
        {
            foreach (var item in _listStudent)
            {
                await _studentService.Add(item);
            }
            var result = await _studentService.GetAll();
            Assert.True(result.Count == 2);
            Assert.That(result[0].Id, Is.EqualTo(_listStudent[0].Id));
            Assert.That(result[0].Name, Is.EqualTo(_listStudent[0].Name));
            Assert.That(result[0].Dob, Is.EqualTo(_listStudent[0].Dob));
            Assert.That(result[0].Address, Is.EqualTo(_listStudent[0].Address));
        }
        [Test]
        public async Task UpdateStudent_Test()
        {
            await _studentService.Add(_addStudent);
            // Act
            _addStudent.Name = "Test";
            var existingStudent = await _context.students.FindAsync(_addStudent.Id);
            if (existingStudent != null)
            {
                _context.Entry(existingStudent).State = EntityState.Detached;
            }
            await _studentService.Update(_addStudent);

            // Assert
                var result = await _context.students.FindAsync(_addStudent.Id);
                Assert.NotNull(result);
                Assert.That(result.Name, Is.EqualTo(_addStudent.Name));
                Assert.That(result.Dob, Is.EqualTo(_addStudent.Dob));
                Assert.That(result.Address, Is.EqualTo(_addStudent.Address));
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