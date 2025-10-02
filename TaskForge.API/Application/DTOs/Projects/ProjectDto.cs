using TaskForge.API.Application.DTOs.Users;
using TaskForge.API.Domain.Entities;

namespace TaskForge.API.Application.DTOs.Projects
{
    public record ProjectDto
    {
        public int Id { get; init; }
        public string Name { get; init; }
        public string Description { get; init; }
        public UserDto Owner { get; init; }

        public ProjectDto(Project project)
        {
            Id = project.Id;
            Name = project.Name;
            Description = project.Description;
            Owner = new UserDto(project.Owner);
        }
    }
}
