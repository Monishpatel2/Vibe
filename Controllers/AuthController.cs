using Microsoft.AspNetCore.Mvc;
using Vibe.Api.Data;
using Vibe.Api.Models;
using Vibe.Api.Services;
using System.Security.Cryptography;
using System.Text;

namespace Vibe.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly AuthService _authService;

        public AuthController(AppDbContext context, AuthService authService)
        {
            _context = context;
            _authService = authService;
        }

        [HttpPost("register")]
        public ActionResult Register(User user)
        {
            if (_context.Users.Any(u => u.Email == user.Email))
                return BadRequest("Email already exists");

            // hash password (simple SHA256 for now)
            using var sha = SHA256.Create();
            user.PasswordHash = Convert.ToHexString(sha.ComputeHash(Encoding.UTF8.GetBytes(user.PasswordHash)));

            _context.Users.Add(user);
            _context.SaveChanges();
            return Ok("User registered successfully");
        }

        [HttpPost("login")]
        public ActionResult<string> Login(string email, string password)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == email);
            if (user == null) return Unauthorized("Invalid email");

            using var sha = SHA256.Create();
            var hashed = Convert.ToHexString(sha.ComputeHash(Encoding.UTF8.GetBytes(password)));

            if (user.PasswordHash != hashed) return Unauthorized("Invalid password");

            var token = _authService.GenerateJwtToken(user);
            return Ok(token);
        }
    }
}
