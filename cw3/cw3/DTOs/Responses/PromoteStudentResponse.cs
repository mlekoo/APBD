using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cw3.DTOs.Responses
{
    public class PromoteStudentResponse
    {
        public int idEnrollment { get; set; }
        public string studies { get; set; }
        public int semester { get; set; }
    }
}
