using example1Exam.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace example1Exam.Services
{
    public interface IdbService
    {
        public IEnumerable<Animal> getAnimals(string orderBy);
    }
}
