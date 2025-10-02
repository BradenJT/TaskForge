using TaskForge.API.Application.DTOs.Projects;
using TaskForge.API.Application.DTOs.Tags;
using TaskForge.API.Application.DTOs.Users;
using TaskForge.API.Domain.Entities;
using TaskForge.API.Domain.Enums;

namespace TaskForge.API.Application.DTOs.Tasks
{
    public record TaskDto
    {
        public int Id { get; init; }
        public string Title { get; init; }
        public string? Description { get; init; }
        public DateTime? DueDate { get; init; }
        public TaskPriority Priority { get; init; }
        public ProjectDto Project { get; init; }
        public UserDto? AssignedUser { get; init; }
        public List<TagDto> Tags { get; init; }

        public TaskDto(TaskItem task)
        {
            Id = task.Id;
            Title = task.Title;
            Description = task.Description;
            DueDate = task.DueDate;
            Priority = task.Priority;
            Project = new ProjectDto(task.Project);
            AssignedUser = task.Assignee != null ? new UserDto(task.Assignee) : null;
            Tags = task.Tags.Select(t => new TagDto(t)).ToList();
        }
    }
}
