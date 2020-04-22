using cw3.DAL;
using cw3.DTOs.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace cw3.Controllers
{
    [ApiController]
    [Route("api/enrollments")]
    public class EnrollmentsController : Controller
    {
        private IDbService _service;

        public EnrollmentsController(IDbService _service)
        {
            this._service = _service;
        }

        [HttpPost]
        [Authorize(Roles = "employee")]
        public IActionResult EnrollStudent(EnrollStudentRequest request)
        {
            return _service.EnrollStudent(request);
        }
        [HttpPost("{promotions}")]
        [Authorize(Roles = "employee")]
        public IActionResult PromoteStudent(PromoteStudentRequest request)
        {
            return _service.PromoteStudent(request);
        }
    }
}