namespace RPGOnline.Application.DTOs.Responses
{
    public class TokenResponse
    {
        public string AccessToken { get; set; } = null!;
        public string RefreshToken { get; set; } = null!;
        public string UserRole { get; set; } = null!;
        public string Username { get; set; } = null!;
        public int UId { get; set; }
        public int Avatar { get; set; }
    }
}
