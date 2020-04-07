using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cw3.DTOs.Requests
{
    public class PromoteStudentRequest
    {
        public string studies { get; set; }
        public int semester { get; set; }
    }
}
