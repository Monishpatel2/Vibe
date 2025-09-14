    using Microsoft.AspNetCore.Mvc;
    using Vibe.Api.Data;
    using Vibe.Api.Models;

    namespace Vibe.Api.Controllers
    {
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
        }
    }
