namespace TaskForge.API.Application.DTOs.Tags
{
    public class CreateTagRequest(string Name, string Color)
    {
        public string Name { get; } = Name;
        public string Color { get; } = Color;
    }
}
