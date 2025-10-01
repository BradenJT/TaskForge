namespace TaskForge.API.Domain.Entities
{
    public class Tag
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;
        // Navigation property for many-to-many relationship with TaskItem
        public ICollection<TaskItem> TaskItems { get; set; } = new List<TaskItem>();
    }
}
