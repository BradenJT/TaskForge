namespace TaskForge.API.Application.DTOs.Projects
{
    public record CreateProjectRequest(string Name, string Description)
    {
        public string Name { get; } = Name;
        public string Description { get; } = Description;
    }
}
