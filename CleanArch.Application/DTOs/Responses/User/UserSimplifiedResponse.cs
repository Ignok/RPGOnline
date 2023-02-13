namespace RPGOnline.Application.DTOs.Responses.User
{
    public class UserSimplifiedResponse
    {
        public int UId { get; set; }
        public string Username { get; set; } = null!;
        public int Picture { get; set; }
        public bool IsFollowed { get; set; } = false;
    }
}
