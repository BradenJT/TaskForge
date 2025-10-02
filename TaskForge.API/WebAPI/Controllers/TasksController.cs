using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskForge.API.Application.DTOs.Tasks;
using TaskForge.API.Domain.Entities;
using TaskForge.API.Infrastructure.Database;

namespace TaskForge.API.WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class TasksController(
        ApplicationDbContext dbContext) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetTasks([FromQuery] bool dueThisWeek = false, [FromQuery] int? assignedTo = null)
        {
            var query = dbContext.Tasks
                .Include(t => t.Project)
                .Include(t => t.Assignee)
                .Include(t => t.Tags)
                .AsQueryable();

            if (dueThisWeek)
            {
                var startOfWeek = DateTime.UtcNow.Date;
                var endOfWeek = startOfWeek.AddDays(7);
                query = query.Where(t => t.DueDate >= startOfWeek && t.DueDate < endOfWeek);
            }

            if (assignedTo.HasValue)
                query = query.Where(t => t.AssigneeId == assignedTo.Value);

            var tasks = await query.Select(t => new TaskDto(t)).ToListAsync();
            return Ok(tasks);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTask(int id)
        {
            var task = await dbContext.Tasks
                .Include(t => t.Project)
                .Include(t => t.Assignee)
                .Include(t => t.Tags)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (task == null)
                return NotFound();

            return Ok(new TaskDto(task));
        }

        [HttpPost]
        public async Task<IActionResult> CreateTask([FromBody] CreateTaskRequest request)
        {
            var task = new TaskItem
            {
                Title = request.Title,
                Description = request.Description,
                DueDate = request.DueDate,
                Priority = (Domain.Enums.TaskPriority)request.Priority,
                ProjectId = request.ProjectId,
                AssigneeId = request.AssignedUserId
            };

            if (request.TagIds != null && request.TagIds.Any())
            {
                var tags = await dbContext.Tags.Where(t => request.TagIds.Contains(t.Id)).ToListAsync();
                foreach (var tag in tags)
                    task.Tags.Add(tag);
            }

            dbContext.Tasks.Add(task);
            await dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTask), new { id = task.Id }, new TaskDto(task));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask(int id, [FromBody] UpdateTaskRequest request)
        {
            var task = await dbContext.Tasks
                .Include(t => t.Tags)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (task == null)
                return NotFound();

            task.Title = request.Title ?? task.Title;
            task.Description = request.Description ?? task.Description;
            task.DueDate = request.DueDate ?? task.DueDate;
            task.Priority = request.Priority.HasValue ? (Domain.Enums.TaskPriority)request.Priority.Value : task.Priority;
            task.AssigneeId = request.AssignedUserId ?? task.AssigneeId;

            if (request.TagIds != null)
            {
                task.Tags.Clear();
                var tags = await dbContext.Tags.Where(t => request.TagIds.Contains(t.Id)).ToListAsync();
                foreach (var tag in tags)
                    task.Tags.Add(tag);
            }

            await dbContext.SaveChangesAsync();
            return Ok(new TaskDto(task));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            var task = await dbContext.Tasks.FindAsync(id);
            if (task == null)
                return NotFound();

            dbContext.Tasks.Remove(task);
            await dbContext.SaveChangesAsync();
            return NoContent();
        }
    }
}
