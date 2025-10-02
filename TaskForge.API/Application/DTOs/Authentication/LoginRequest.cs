namespace TaskForge.API.Application.DTOs.Authentication
{
    public record LoginRequest(string Email, string Password)
    {
        public string Email { get; init; } = Email;
        public string Password { get; init; } = Password;
    }
}
