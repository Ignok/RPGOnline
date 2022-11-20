using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGOnline.Application.DTOs.Responses
{
    public class PostResponse
    {
        public int PostId { get; set; }
        public string Title { get; set; } = null!;
        public string Content { get; set; } = null!;
        public string? Picture { get; set; }
        public DateTime CreationDate { get; set; }

        public int Likes { get; set; } 

        public virtual ICollection<UserResponse> UIds { get; set; }

        public virtual UserResponse CreatorNavigation { get; set; } = null!;
        public virtual ICollection<CommentResponse> Comments { get; set; } =  new HashSet<CommentResponse>();
    }
}
