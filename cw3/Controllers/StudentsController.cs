using System;
using Cw3.DAL;
using Cw3.Models;
using Microsoft.AspNetCore.Mvc;

namespace Cw3.Controllers
{
    [ApiController]
    [Route("api/students")]
    public class StudentsController : ControllerBase
    {
        private readonly IDbService _dbService;

        public StudentsController(IDbService dbService)
        {
            _dbService = dbService;
        }
        [HttpGet()]
        public IActionResult GetStudents(string orderBy)
        {
            return Ok(_dbService.GetStudents());
        }

        [HttpGet("{id}")]
        public IActionResult GetStudent(int id)
        {
            if(id == 1)
            {
                return Ok("Tolin");
            }
            else if(id == 2)
            {
                return Ok("Tybrel");
            }
            else if (id == 3)
            {
                return Ok("Dyrdun");
            }
            return NotFound("Nie znaleziono studenta...");
        }

        [HttpPost]
        public IActionResult CreateStudent(Student student)
        {
            student.IndexNumber = $"s{new Random().Next(1, 2000)}";
            return Ok(student);
        }

        [HttpPut("{id}")]
        public IActionResult PutStudent(int id)
        {         
            return Ok("Student zaktualizowany");
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteStudent(int id)
        {
            //co tu dodac bez bazy danych?
            return Ok("Student usunięty");
        }
    }
}