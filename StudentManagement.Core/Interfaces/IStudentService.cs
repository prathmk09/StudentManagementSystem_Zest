using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentManagement.Core.DTOs;
using StudentManagement.Core.Entities;

namespace StudentManagement.Core.Interfaces
{
    public interface IStudentService
    {
        Task<IEnumerable<Student>> GetAllStudentsAsync();
        Task<Student?> GetStudentByIdAsync(int id);
        Task AddStudentAsync(StudentDto studentDto);
        Task UpdateStudentAsync(int id, StudentDto studentDto);
        Task DeleteStudentAsync(int id);
    }
}
