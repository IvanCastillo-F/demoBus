using BusProyectApi.Data;
using BusProyectApi.Models;
using BusProyectApi.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BusProyectApi.Controllers {
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase {
        private readonly ApplicationDBContext _context;
        public UserController(ApplicationDBContext context) {
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
        [HttpPost]
        public async Task<ActionResult<User>> CreateUser(User user) {
            if (_context.users.Any(u => u.Username == user.Username)) {
                return Conflict("User with this username already exists");
            }

            // If there are no duplicates, create the user.
            _context.users.Add(user);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetUsers), new { id = user.Id }, user);
        }

        // UPDATE USER
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, User user) {

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
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id) {
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