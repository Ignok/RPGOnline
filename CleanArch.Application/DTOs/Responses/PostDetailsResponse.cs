using RPGOnline.Application.DTOs.Responses.User;

namespace RPGOnline.Application.DTOs.Responses
{
    public class PostDetailsResponse
    {
        public int PostId { get; set; }
        public string Title { get; set; } = null!;
        public string Content { get; set; } = null!;
        public string? Picture { get; set; }
        public DateTime CreationDate { get; set; }
        public string? Tag { get; set; }

        public int Likes { get; set; }

        public bool IsLiked { get; set; } = false;

        public virtual UserSimplifiedResponse CreatorNavigation { get; set; } = null!;
        public virtual ICollection<CommentResponse> Comments { get; set; } = new HashSet<CommentResponse>();

    }
}
