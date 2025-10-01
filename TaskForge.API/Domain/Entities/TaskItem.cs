using TaskForge.API.Domain.Enums;

namespace TaskForge.API.Domain.Entities
{
    public class TaskItem
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? DueDate { get; set; }
        public TaskPriority Priority { get; set; } = TaskPriority.Medium;
        public bool IsCompleted { get; set; } = false;
        // Foreign key for User (Assignee)
        public int? AssigneeId { get; set; }
        public User? Assignee { get; set; }
        // Foreign key for Project
        public int ProjectId { get; set; }
        public Project Project { get; set; } = null!;
        // Many-to-many relationship with Tag
        public ICollection<Tag> Tags { get; set; } = new List<Tag>();
    }
}
