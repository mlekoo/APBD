using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using example1Exam.Models;
using example1Exam.Services;
using Microsoft.AspNetCore.Mvc;

namespace example1Exam.Controllers
{
    [ApiController]
    [Route("api")]
    public class AnimalsController : ControllerBase
    {
        private readonly IdbService _dbService;

        public AnimalsController(IdbService dbService)
        {
            _dbService = dbService;
        }

        [HttpGet("/getAnimals/{sortBy}")]
        public IActionResult getAnimals(string sortBy)
        {

            IEnumerable<Animal> animals = _dbService.getAnimals(sortBy);




            return Ok(animals);
        }
    }
}