namespace TaskForge.API.Domain.Entities
{
    public class Project
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public int OwnerId { get; set; }
        public User Owner { get; set; } = null!;
        // Navigation property for related TaskItems
        public ICollection<TaskItem> TaskItems { get; set; } = new List<TaskItem>();
    }
}
