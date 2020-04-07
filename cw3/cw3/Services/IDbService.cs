using cw3.DTOs.Requests;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cw3.DAL
{
    public interface IDbService
    {
        public IEnumerable<Student> GetStudents();
        public Student UpdateStudent(string id, string varType, string value);
        public void DeleteStudent(string id);
        IEnumerable<Enrollment> GetEnrollment(string index);
        public Student GetStudent(string index);
        public IActionResult EnrollStudent(EnrollStudentRequest request);
        public IActionResult PromoteStudent(PromoteStudentRequest request);

        
    }
}
