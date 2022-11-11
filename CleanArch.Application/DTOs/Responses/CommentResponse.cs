using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGOnline.Application.DTOs.Responses
{
    public class CommentResponse
    {
        public int? ResponseCommentId { get; set; }

        public UserResponse UserResponse { get; set; }

        public string Content { get; set; } = null!;
        public DateTime CreationDate { get; set; }

        public int PostIdNavigation { get; set; }
    }
}
