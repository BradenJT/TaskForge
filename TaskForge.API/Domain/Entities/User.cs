namespace TaskForge.API.Domain.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string Role { get; set; } = "User"; // Default role // Admin, Manager, User
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        // Navigation property for related TaskItems (as Assignee)
        public ICollection<TaskItem> AssignedTasks { get; set; } = new List<TaskItem>();
        // Navigation property for related Projects (as Owner)
        public ICollection<Project> OwnedProjects { get; set; } = new List<Project>();
    }
}
