using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskForge.API.Application.DTOs.Projects;
using TaskForge.API.Infrastructure.Database;

namespace TaskForge.API.WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class ProjectsController(
        ApplicationDbContext dbContext) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetProjects()
        {
            var projects = await dbContext.Projects
                .Include(p => p.Owner)
                .Select(p => new ProjectDto(p))
                .ToListAsync();
            return Ok(projects);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProject(int id)
        {
            var project = await dbContext.Projects
                .Include(p => p.Owner)
                .FirstOrDefaultAsync(p => p.Id == id);
            if (project == null)
                return NotFound();
            return Ok(new ProjectDto(project));
        }

        [HttpPost]
        public async Task<IActionResult> CreateProject([FromBody] CreateProjectRequest request)
        {
            var currentUserId = int.Parse(User.FindFirst("sub")?.Value ?? "0");
            var project = new Domain.Entities.Project
            {
                Name = request.Name,
                Description = request.Description,
                OwnerId = currentUserId
            };
            dbContext.Projects.Add(project);
            await dbContext.SaveChangesAsync();
            return CreatedAtAction(nameof(GetProject), new { id = project.Id }, new ProjectDto(project));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProject(int id, [FromBody] UpdateProjectRequest request)
        {
            var project = await dbContext.Projects.FindAsync(id);
            if (project == null)
                return NotFound();
            var currentUserId = int.Parse(User.FindFirst("sub")?.Value ?? "0");
            var currentUserRole = User.FindFirst("role")?.Value;
            if (project.OwnerId != currentUserId && currentUserRole != "Admin")
                return Forbid();
            project.Name = request.Name ?? project.Name;
            project.Description = request.Description ?? project.Description;
            await dbContext.SaveChangesAsync();
            return Ok(new ProjectDto(project));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProject(int id)
        {
            var project = await dbContext.Projects.FindAsync(id);
            if (project == null)
                return NotFound();
            var currentUserId = int.Parse(User.FindFirst("sub")?.Value ?? "0");
            var currentUserRole = User.FindFirst("role")?.Value;
            if (project.OwnerId != currentUserId && currentUserRole != "Admin")
                return Forbid();
            dbContext.Projects.Remove(project);
            await dbContext.SaveChangesAsync();
            return NoContent();
        }
    }
}
