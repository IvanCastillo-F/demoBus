using BusProyectApi.Data;
using BusProyectApi.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BusProyectApi.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : BaseController {
        private readonly ApplicationDBContext _context;
        public UserController(ApplicationDBContext context) : base(context) {
            _context = context;
        }

        // GET ALL USERS
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers() {
            return await _context.users.ToListAsync();
        }

        // GET A SPECIFIC USER
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id) {
            var user = await _context.users.FindAsync(id);
            if (user == null) {
                return NotFound();
            }
            return user;
        }

        // CREATE USER
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<User>> CreateUser(User user) {
            // if (!await IsAdmin()) {
            //     return StatusCode(403, new { message = "Only admins can create users." });
            // }

            if (_context.users.Any(u => u.Username == user.Username)) {
                return Conflict("User with this username already exists");
            }

            // If there are no duplicates, create the user.
            _context.users.Add(user);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetUsers), new { id = user.Id }, user);
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginModel loginModel)
        {
            // Hardcode a simple user check (in a real scenario, you would query the database)
            if (loginModel.Username == "MirandaLawson" && loginModel.Password == "CerberusOfficer") {
            // Simulate the user object with IsAdmin flag
            var user = new User
            {
                Id = 1,
                Username = "MirandaLawson",
                IsAdmin = true // Set to true for testing admin access
            };

            // Generate JWT token for the hardcoded user
            var token = TokenService.GenerateToken(user.Id, user.IsAdmin);
            
            return Ok(new { token });
            } else {
                return Unauthorized("Invalid credentials."); 
            }
        }

        public class LoginModel
        {
            public required string Username { get; set; }
            public required string Password { get; set; }
        }



        // UPDATE USER
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, User user) {

            // if (!await IsAdmin()) {
            //     return Forbid();
            // }

            var existingUser = await _context.users.FindAsync(id);
            if (existingUser == null) {
                return NotFound();
            }

            // if (_context.users.Any(u => u.Username == u.Username)) {
            //     return Conflict("User with this username already exists");
            // }

            existingUser.Username = user.Username;
            existingUser.Password = user.Password;
            existingUser.IsAdmin = user.IsAdmin;

            try {
                await _context.SaveChangesAsync();
            } catch (DbUpdateConcurrencyException) {
                if (!_context.users.Any(u => u.Id == id)) {
                    return NotFound();
                } throw;
            }
            return NoContent();
        }

        // DELETE USER
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id) {
            // if (!await IsAdmin()) {
            //     return Forbid();
            // }
            var user = await _context.users.FindAsync(id);

            if (user == null) {
                return NotFound(new { message = $"Error! User with ID {id} not found!" });
            }
            _context.users.Remove(user);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}