namespace RPGOnline.Application.DTOs.Requests.Forum
{
    public class CommentRequest
    {
        public int? ResponseCommentId { get; set; }
        public int UId { get; set; }
        public string Content { get; set; } = null!;
    }
}
