using Microsoft.AspNetCore.Mvc;
using Vibe.Api.Data;
using Vibe.Api.Models;
using Microsoft.AspNetCore.Authorization;

namespace Vibe.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UsersController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/users
        [HttpGet]
        public ActionResult<IEnumerable<User>> GetUsers()
        {
            return Ok(_context.Users.ToList());
        }

        // POST: api/users
        [HttpPost]
        public ActionResult<User> CreateUser(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetUsers), new { id = user.UserId }, user);
        }

        // GET: api/users/{id}
        [HttpGet("{id}")]
        public ActionResult<User> GetUser(int id)
        {
            var user = _context.Users.Find(id);
            if (user == null) return NotFound();
            return Ok(user);
        }

        // PUT: api/users/{id}
        [HttpPut("{id}")]
        public ActionResult UpdateUser(int id, User updatedUser)
        {
            var user = _context.Users.Find(id);
            if (user == null) return NotFound();

            user.Username = updatedUser.Username;
            user.Email = updatedUser.Email;
            user.Bio = updatedUser.Bio;

            _context.SaveChanges();
            return NoContent();
        }

        // DELETE: api/users/{id}
        [HttpDelete("{id}")]
        public ActionResult DeleteUser(int id)
        {
            var user = _context.Users.Find(id);
            if (user == null) return NotFound();

            _context.Users.Remove(user);
            _context.SaveChanges();
            return NoContent();
        }
    }
}
