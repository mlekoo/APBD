using Cw3.Models;
using System.Collections.Generic;

namespace cw3.DAL
{
    public interface IDbService
    {
        public IEnumerable<Student> GetStudents();
        public IEnumerable<Enrollment> GetStudentEnrollment(string id);
    }
}