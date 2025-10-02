namespace TaskForge.API.Application.DTOs.Projects
{
    public record UpdateProjectRequest(string? Name, string Description)
    {
        public string? Name { get; } = Name;
        public string Description { get; } = Description;
    }
}
