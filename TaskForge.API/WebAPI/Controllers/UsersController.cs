using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskForge.API.Application.DTOs.Users;
using TaskForge.API.Infrastructure.Database;

namespace TaskForge.API.WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class UsersController(
        ApplicationDbContext dbContext) : ControllerBase
    {
        /// <summary>
        /// Get all users (Admin only)
        /// </summary>
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetUsers()
        {
            var users = await dbContext.Users
                .Select(u => new UserDto(u))
                .ToListAsync();
            return Ok(users);
        }

        /// <summary>
        /// Get a single user by ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await dbContext.Users.FindAsync(id);
            if (user == null)
                return NotFound();

            return Ok(new UserDto(user));
        }

        /// <summary>
        /// Update a user
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UpdateUserRequest request)
        {
            var user = await dbContext.Users.FindAsync(id);
            if (user == null)
                return NotFound();

            // Optional: only allow updating self or Admin updating anyone
            var currentUserId = int.Parse(User.FindFirst("sub")?.Value ?? "0");
            var currentUserRole = User.FindFirst("role")?.Value;

            if (currentUserId != id && currentUserRole != "Admin")
                return Forbid();

            user.Username = request.Username ?? user.Username;
            user.Email = request.Email ?? user.Email;

            if (!string.IsNullOrEmpty(request.Password))
            {
                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
            }

            await dbContext.SaveChangesAsync();

            return Ok(new UserDto(user));
        }

        /// <summary>
        /// Delete a user (Admin only)
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await dbContext.Users.FindAsync(id);
            if (user == null)
                return NotFound();

            dbContext.Users.Remove(user);
            await dbContext.SaveChangesAsync();

            return NoContent();
        }
    }
}
