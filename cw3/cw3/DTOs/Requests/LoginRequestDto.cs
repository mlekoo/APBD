using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cw3.DTOs.Requests
{
    public class LoginRequestDto
    {
        public string Login { get; set; }
        public string Password { get; set; }

        public string Salt { get; set; }

       
    }
}
