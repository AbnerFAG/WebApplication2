using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Data;
using WebApplication1.Models;
using WebApplication1.Requests;
using WebApplication1.Responses;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class StudentsCustomController : ControllerBase
    {
        private readonly DemoContext _context;

        public StudentsCustomController(DemoContext context)
        {
            _context = context;
        }

        [HttpPost]
        public List<StudentResponse> SearchByFullNameEmail(StudentSearchRequest request)
        {
            var students = _context.Student
                .Include(s => s.Grade)
                .Where(s =>
                    (string.IsNullOrEmpty(request.FirstName) || s.FirstName.Contains(request.FirstName)) &&
                    (string.IsNullOrEmpty(request.LastName) || s.LastName.Contains(request.LastName)) &&
                    (string.IsNullOrEmpty(request.Email) || s.Email.Contains(request.Email))
                )
                .OrderByDescending(s => s.LastName)
                .ToList();

            var response = students.Select(s => new StudentResponse
            {
                StudentId = s.StudentId,
                FullName = $"{s.FirstName} {s.LastName}",
                Email = s.Email,
                GradeName = s.Grade.Description
            }).ToList();

            return response;
        }

        [HttpPost]
        public List<StudentResponse> SearchByStudentNameAndGrade(StudentGradeSearchRequest request)
        {
            var students = _context.Student
                .Include(s => s.Grade)
                .Where(s =>
                    (string.IsNullOrEmpty(request.StudentName) ||
                     s.FirstName.Contains(request.StudentName) ||
                     s.LastName.Contains(request.StudentName)) &&
                    (string.IsNullOrEmpty(request.GradeName) ||
                     s.Grade.Description.Contains(request.GradeName))
                )
                .OrderByDescending(s => s.Grade.Name)
                .ToList();

            var response = students.Select(s => new StudentResponse
            {
                StudentId = s.StudentId,
                FullName = $"{s.FirstName} {s.LastName}",
                Email = s.Email,
                GradeName = s.Grade.Description
            }).ToList();

            return response;
        }

        [HttpPost]
        public List<StudentEnrollmentResponse> SearchByCourseName(StudentByCourseRequest request)
        {
            var enrollments = _context.Enrollment
                .Include(e => e.Student)
                .Include(e => e.Course)
                .Where(e => e.Course.Name.Contains(request.CourseName))
                .OrderBy(e => e.Course.Name)
                .ThenBy(e => e.Student.LastName)
                .ToList();

            var response = enrollments.Select(e => new StudentEnrollmentResponse
            {
                StudentFullName = $"{e.Student.FirstName} {e.Student.LastName}",
                CourseName = e.Course.Name,
                EnrollmentDate = e.Date
            }).ToList();

            return response;
        }

        [HttpPost]
        public List<StudentEnrollmentResponse> SearchByGradeName(StudentByGradeRequest request)
        {
            var enrollments = _context.Enrollment
                .Include(e => e.Student).ThenInclude(s => s.Grade)
                .Include(e => e.Course)
                .Where(e => e.Student.Grade.Description.Contains(request.GradeName))
                .OrderBy(e => e.Course.Name)
                .ThenBy(e => e.Student.LastName)
                .ToList();

            var response = enrollments.Select(e => new StudentEnrollmentResponse
            {
                StudentFullName = $"{e.Student.FirstName} {e.Student.LastName}",
                CourseName = e.Course.Name,
                EnrollmentDate = e.Date
            }).ToList();

            return response;
        }
    }
}
