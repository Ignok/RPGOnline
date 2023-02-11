namespace RPGOnline.Application.DTOs.Responses
{
    public class UserFriendsResponse
    {
        public int UId { get; set; }
        public string Username { get; set; } = null!;
        public int Picture { get; set; }
        public int Status { get; set; }
    }
}
