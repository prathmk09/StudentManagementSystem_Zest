using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentManagement.Core.DTOs;
using StudentManagement.Core.Entities;
using StudentManagement.Core.Interfaces;

namespace StudentManagement.Service
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _repository;

        public StudentService(IStudentRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Student>> GetAllStudentsAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Student?> GetStudentByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task AddStudentAsync(StudentDto studentDto)
        {
            var student = new Student
            {
                Name = studentDto.Name,
                Email = studentDto.Email,
                Age = studentDto.Age,
                Course = studentDto.Course,
                CreatedDate = DateTime.UtcNow
            };
            await _repository.AddAsync(student);
        }

        public async Task UpdateStudentAsync(int id, StudentDto studentDto)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null)
                throw new KeyNotFoundException($"Student with ID {id} not found.");

            existing.Name = studentDto.Name;
            existing.Email = studentDto.Email;
            existing.Age = studentDto.Age;
            existing.Course = studentDto.Course;

            await _repository.UpdateAsync(existing);
        }

        public async Task DeleteStudentAsync(int id)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null)
                throw new KeyNotFoundException($"Student with ID {id} not found.");

            await _repository.DeleteAsync(id);
        }
    }
}
