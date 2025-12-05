using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentManagementApp.Application.Services;
using StudentManagementApp.Domain.Interfaces;
using System.Security.Claims;

namespace StudentManagementAppASP.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        
        private readonly IUnitOfWork _uow;

        public AuthController(ITokenService tokenService, IUnitOfWork uow)
        {
            _tokenService = tokenService;
            _uow = uow;
        }

        public class LoginDto
        {
            public string Email { get; set; } = default!;
            public string Password { get; set; } = default!;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            if (dto == null) return BadRequest();

            
            var teacher = (await _uow.Teachers.GetAllAsync()).FirstOrDefault(t => t.Email == dto.Email);
            if (teacher != null)
            {
                
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, teacher.Id.ToString()),
                    new Claim(ClaimTypes.Name, teacher.Name),
                    new Claim(ClaimTypes.Role, "Teacher"),
                    new Claim(ClaimTypes.Email, teacher.Email)
                };

                var token = _tokenService.GenerateToken(claims);
                return Ok(new { token });
            }

            var student = (await _uow.Students.GetAllAsync()).FirstOrDefault(s => s.Email == dto.Email);
            if (student != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, student.Id.ToString()),
                    new Claim(ClaimTypes.Name, $"{student.FirstName} {student.LastName}"),
                    new Claim(ClaimTypes.Role, "Student"),
                    new Claim(ClaimTypes.Email, student.Email)
                };

                var token = _tokenService.GenerateToken(claims);
                return Ok(new { token });
            }

            return Unauthorized("Invalid credentials");
        }
    }
}
