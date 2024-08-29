namespace Task3.DTOs
{
    public class UsersRequestDTO
    {
        public string Username { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string Password { get; set; } = null!;

        public string? Phone { get; set; }

        public IFormFile? Image { get; set; }
    }
}
