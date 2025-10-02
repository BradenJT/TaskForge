namespace TaskForge.API.Application.DTOs.Tags
{
    public record TagDto
    {
        public int Id { get; init; }
        public string Name { get; init; }
        public string Color { get; init; }
        public TagDto(Domain.Entities.Tag tag)
        {
            Id = tag.Id;
            Name = tag.Name;
            Color = tag.Color;
        }
    }
}
