using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cw3.DAL;
using Microsoft.AspNetCore.Mvc;


namespace cw3.Controllers
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
        [HttpGet]
        public IActionResult GetStudents()
        {
            return Ok(_dbService.GetStudents());
        }

        [HttpGet("{index}")]
        public IActionResult GetStudent(string index)
        {
            return Ok(_dbService.GetStudent(index));
        }
        [HttpGet("getEnrollment/{index}")]
        public IActionResult getStudentEnrollment(string index)
        {
            return Ok(_dbService.GetEnrollment(index));
        }
        [HttpPost]
        public IActionResult CreateStudent(Student student)
        {
            student.index = $"s{new Random().Next(1, 2000)}";
            return Ok(student);
        }
        [HttpPut("{id}")]
        public IActionResult UpdateStudent(string id, string varType, string value)
        {   
            return Ok($"Zaktualizowano studenta {_dbService.UpdateStudent(id, varType, value)}");
        }
        [HttpDelete("{id}")]
        public IActionResult DeleteStudent(string id)
        {
            _dbService.DeleteStudent(id);
            return Ok($"Usunięto studenta o id: {id}");
        }
    }
}