using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using StudentClass.Application.Interfaces;
using StudentClass.Application.ViewModels;

using StudentClass.Infrastructure.Data;

using StudentClass.Infrastructure.Services;




namespace StudentClass.Test
{
    [TestFixture]
    public class StudentTest
    {
        private DbContextOptions<DatabaseDbContext> _options;
        private DatabaseDbContext _context;
        private StudentViewModel.Student _student;
        private List<StudentViewModel.Student> _listStudents;
        private StudentService _studentService;
       
        [SetUp]
        public void Setup()
        {
            _options = new DbContextOptionsBuilder<DatabaseDbContext>()
                .UseInMemoryDatabase("TestDatabase")
                .Options;
            _context = new DatabaseDbContext(_options);
            _studentService = new StudentService(_context);
            _student = new StudentViewModel.Student()
            {
                Id = 1,
                Name = "Test",
                Dob = new DateTime(2023, 1, 1),
                Address = "Hà Nội",
                PhoneNumber = "1234567890",
            };
            _listStudents = new List<StudentViewModel.Student>()
            {
                new StudentViewModel.Student{ Id = 1,Name="Student 1",Address ="Hà Nội",PhoneNumber ="0123456789"},
                new StudentViewModel.Student{ Id = 2,Name="Student 2",Address ="Hà Nội",PhoneNumber ="0123456788"}
            };
        }

        [Test]

        public async Task TestAddStudentAsync()
        {

            await _studentService.Add(_student);

            // Assert
            var result = _context.Student.FirstOrDefault(c => c.Id == _student.Id);
            Assert.NotNull(result);
            Assert.That(result.Name, Is.EqualTo(_student.Name));
            Assert.That(result.Dob, Is.EqualTo(_student.Dob));
            Assert.That(result.Address, Is.EqualTo(_student.Address));
        }
        [Test]
        public async Task GetById_Test()
        {
            await _studentService.Add(_student);
            var result = await _studentService.GetById(_student.Id);
            Assert.NotNull(result);
            Assert.That(result.Id, Is.EqualTo(_student.Id));
            Assert.That(result.Name, Is.EqualTo(_student.Name));
            Assert.That(result.Dob, Is.EqualTo(_student.Dob));
            Assert.That(result.Address, Is.EqualTo(_student.Address));
        }
        [Test]
        public async Task GetAll_Test()
        {
            foreach (var item in _listStudents)
            {
                await _studentService.Add(item);
            }
            var result = await _studentService.GetAll();
            Assert.True(result.Count() == 2);
            Assert.That(result[0].Id, Is.EqualTo(_listStudents[0].Id));
            Assert.That(result[0].Name, Is.EqualTo(_listStudents[0].Name));
        }
        [Test]
        public async Task UpdateStudent_Test()
        {
            await _studentService.Add(_student);
            // Act
            _student.Name = "Test";
            var existingStudent = await _context.Student.FindAsync(_student.Id);
            if (existingStudent != null)
            {
                _context.Entry(existingStudent).State = EntityState.Detached;
            }
            await _studentService.Update(_student);

            // Assert
            var result = await _context.Student.FindAsync(_student.Id);
            Assert.NotNull(result);
            Assert.That(result.Name, Is.EqualTo(_student.Name));
            Assert.That(result.Dob, Is.EqualTo(_student.Dob));
            Assert.That(result.Address, Is.EqualTo(_student.Address));
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