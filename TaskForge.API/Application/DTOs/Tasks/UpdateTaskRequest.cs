namespace TaskForge.API.Application.DTOs.Tasks
{
    public record UpdateTaskRequest(string? Title, string? Description, DateTime? DueDate, int? Priority, int? ProjectId, int? AssignedUserId, List<int>? TagIds)
    {
        public string? Title { get; init; } = Title;
        public string? Description { get; init; } = Description;
        public DateTime? DueDate { get; init; } = DueDate;
        public int? Priority { get; init; } = Priority;
        public int? ProjectId { get; init; } = ProjectId;
        public int? AssignedUserId { get; init; } = AssignedUserId;
        public List<int>? TagIds { get; init; } = TagIds ?? new List<int>();
    }
}
