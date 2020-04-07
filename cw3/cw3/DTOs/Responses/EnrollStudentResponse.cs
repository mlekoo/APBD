using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cw3.DTOs.Responses
{
    public class EnrollStudentResponse
    {
        public string lastName { get; set; }
        public int semester { get; set; }
        public DateTime startDate { get; set; }
    }
}
