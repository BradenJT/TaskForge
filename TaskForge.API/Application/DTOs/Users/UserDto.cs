using TaskForge.API.Domain.Entities;

namespace TaskForge.API.Application.DTOs.Users
{
    public record UserDto
    {
        public int Id { get; init; }
        public string Username { get; init; }
        public string Email { get; init; }
        public string Role { get; init; }

        public UserDto(User user)
        {
            Id = user.Id;
            Username = user.Username;
            Email = user.Email;
            Role = user.Role;
        }
    }
}
