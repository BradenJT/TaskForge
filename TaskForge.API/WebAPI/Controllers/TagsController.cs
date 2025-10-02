using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskForge.API.Application.DTOs.Tags;
using TaskForge.API.Infrastructure.Database;

namespace TaskForge.API.WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class TagsController(
        ApplicationDbContext dbContext) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetTags()
        {
            var tags = await dbContext.Tags
                .Select(t => new TagDto(t))
                .ToListAsync();
            return Ok(tags);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTag([FromBody] CreateTagRequest request)
        {
            var existingTag = await dbContext.Tags
                .FirstOrDefaultAsync(t => t.Name.ToLower() == request.Name.ToLower());
            if (existingTag != null)
                return Conflict("A tag with the same name already exists.");
            var tag = new Domain.Entities.Tag
            {
                Name = request.Name,
                Color = request.Color
            };
            dbContext.Tags.Add(tag);
            await dbContext.SaveChangesAsync();
            return CreatedAtAction(nameof(GetTags), new { id = tag.Id }, new TagDto(tag));
        }
    }
}
