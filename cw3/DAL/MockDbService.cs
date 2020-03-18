using Cw3.Models;
using System.Collections.Generic;

namespace Cw3.DAL
{
    public class MockDbService : IDbService
    {
        private static IEnumerable<Student> _students;

        static MockDbService()
        {
            _students = new List<Student>
            {
                new Student { IdStudent = 1, FirstName = "Miłosz", LastName = "Sosnowski" },
                new Student { IdStudent = 2, FirstName = "Kuba", LastName = "Zagajewski" },
                new Student { IdStudent = 3, FirstName = "Michał", LastName = "Ferendowicz" }
            };
        }

            public IEnumerable<Student> GetStudents()
            {
            return _students;
            }
        }
    }
