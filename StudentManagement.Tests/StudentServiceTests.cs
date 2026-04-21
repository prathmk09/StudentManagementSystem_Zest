using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using StudentManagement.Core.DTOs;
using StudentManagement.Core.Entities;
using StudentManagement.Core.Interfaces;
using StudentManagement.Service;

namespace StudentManagement.Tests
{
    public class StudentServiceTests
    {
        private readonly Mock<IStudentRepository> _mockRepo;
        private readonly StudentService _studentService;

        public StudentServiceTests()
        {
            _mockRepo = new Mock<IStudentRepository>();
            _studentService = new StudentService(_mockRepo.Object);
        }

        
        [Fact]
        public async Task GetAllStudentsAsync_ShouldReturnAllStudents()
        {
           
            var students = new List<Student>
            {
                new Student { Id = 1, Name = "Prathmesh", Email = "prathmesh@gmail.com", Age = 22, Course = "DFS.Net" },
                new Student { Id = 2, Name = "Rohit", Email = "jadhav.com", Age = 25, Course = "Angular" }
            };
            _mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(students);

           
            var result = await _studentService.GetAllStudentsAsync();

           
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }

        
        [Fact]
        public async Task GetStudentByIdAsync_ShouldReturnStudent_WhenExists()
        {
           
            var student = new Student { Id = 1, Name = "Prathmesh", Email = "prathmesh@gmail.com", Age = 22, Course = "DFS.Net" };
            _mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(student);

            
            var result = await _studentService.GetStudentByIdAsync(1);

           
            Assert.NotNull(result);
            Assert.Equal("Prathmesh", result.Name);
        }

        [Fact]
        public async Task GetStudentByIdAsync_ShouldReturnNull_WhenNotExists()
        {
            
            _mockRepo.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Student?)null);

        
            var result = await _studentService.GetStudentByIdAsync(99);

            
            Assert.Null(result);
        }

        
        [Fact]
        public async Task AddStudentAsync_ShouldCallRepository_Once()
        {
            
            var studentDto = new StudentDto
            {
                Name = "Prathmesh",
                Email = "prathmesh@gmail.com",
                Age = 22,
                Course = "DFS.Net"
            };
            _mockRepo.Setup(r => r.AddAsync(It.IsAny<Student>())).Returns(Task.CompletedTask);

            
            await _studentService.AddStudentAsync(studentDto);

           
            _mockRepo.Verify(r => r.AddAsync(It.IsAny<Student>()), Times.Once);
        }

        
        [Fact]
        public async Task UpdateStudentAsync_ShouldUpdateStudent_WhenExists()
        {
            
            var existing = new Student { Id = 1, Name = "Old Name", Email = "old@gmail.com", Age = 20, Course = "Old Course" };
            var studentDto = new StudentDto { Name = "New Name", Email = "new@gmail.com", Age = 25, Course = "New Course" };

            _mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(existing);
            _mockRepo.Setup(r => r.UpdateAsync(It.IsAny<Student>())).Returns(Task.CompletedTask);

            
            await _studentService.UpdateStudentAsync(1, studentDto);

            
            _mockRepo.Verify(r => r.UpdateAsync(It.IsAny<Student>()), Times.Once);
        }

        [Fact]
        public async Task UpdateStudentAsync_ShouldThrow_WhenNotExists()
        {
           
            _mockRepo.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Student?)null);

            await Assert.ThrowsAsync<KeyNotFoundException>(() =>
                _studentService.UpdateStudentAsync(99, new StudentDto()));
        }

        
        [Fact]
        public async Task DeleteStudentAsync_ShouldDeleteStudent_WhenExists()
        {
            
            var existing = new Student { Id = 1, Name = "Prathmesh", Email = "prathmesh@gmail.com", Age = 22, Course = "DFS.Net" };
            _mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(existing);
            _mockRepo.Setup(r => r.DeleteAsync(1)).Returns(Task.CompletedTask);

           
            await _studentService.DeleteStudentAsync(1);

            
            _mockRepo.Verify(r => r.DeleteAsync(1), Times.Once);
        }

        [Fact]
        public async Task DeleteStudentAsync_ShouldThrow_WhenNotExists()
        {
            
            _mockRepo.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Student?)null);

            
            await Assert.ThrowsAsync<KeyNotFoundException>(() =>
                _studentService.DeleteStudentAsync(99));
        }
    }
}
