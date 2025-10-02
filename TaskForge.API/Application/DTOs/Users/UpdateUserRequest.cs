namespace TaskForge.API.Application.DTOs.Users
{
    public record UpdateUserRequest
    {
        public string? Username { get; init; }
        public string? Email { get; init; }
        public string? Password { get; init; }
    }
}
