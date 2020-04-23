using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using cw3.DAL;
using cw3.DTOs.Requests;
using cw3.Handlers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace cw3.Controllers
{
    [ApiController]
    [Route("api/students")]
    public class StudentsController : ControllerBase
    {
        private readonly IDbService _dbService;
        private IConfiguration Configuration;

        public StudentsController(IDbService dbService, IConfiguration configuration)
        {
            _dbService = dbService;
            this.Configuration = configuration;
        }
        [HttpGet]
        [Authorize]
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

        [HttpPost("login")]
        public IActionResult Login(LoginRequestDto request)
        {


            if (!SqlDbService.CheckStudent(request))
                return StatusCode(403,"Login and password doesn't match");

            var claims = new[]
               {
                new Claim(ClaimTypes.NameIdentifier, "1"),
                new Claim(ClaimTypes.Name, "login"),
                new Claim(ClaimTypes.Role, "admin"),
                new Claim(ClaimTypes.Role, "student"),
                new Claim(ClaimTypes.Role, "employee")

            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["SecretKey"]));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken
                (
                    issuer: "Kacpi",
                    audience: "Students",
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(10),
                    signingCredentials: creds
                );
            var rt = Guid.NewGuid();
            if (!SqlDbService.SaveRefreshToken(request.Login, rt)) {
                return StatusCode(400, "Couldn't save refresh token!");
            };
            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                refreshToken = rt
            }); 
        }
        [HttpPost("token/{Rtoken}")]
        public IActionResult RefreshToken(string Rtoken) {
            var rtk = System.Guid.Parse(Rtoken);
            var newToken = SqlDbService.CheckRefreshToken(rtk);
            if (newToken == null) {

                return StatusCode(403, "Wrong refresh-token");

            }

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, "1"),
                new Claim(ClaimTypes.Name, "login"),
                new Claim(ClaimTypes.Role, "admin"),
                new Claim(ClaimTypes.Role, "student"),
                new Claim(ClaimTypes.Role, "employee")

            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["SecretKey"]));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken
                (
                    issuer: "Kacpi",
                    audience: "Students",
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(10),
                    signingCredentials: creds
                );
            var rt = newToken;

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                refreshToken = rt
            });
        }

        [HttpPost("register")]

        public IActionResult RegisterAccount(Student student) {

            var salt = BasicAuthHandler.CreateSalt();

            var hash = BasicAuthHandler.CreatePass(student.password, salt);


            if (SqlDbService.RegisterAccount(student, salt, hash))
                return Ok();

            return StatusCode(403, "Couldn't create account");
        }

    }
}