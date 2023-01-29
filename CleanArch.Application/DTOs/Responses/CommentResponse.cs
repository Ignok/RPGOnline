using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RPGOnline.Application.DTOs.Responses.User;

namespace RPGOnline.Application.DTOs.Responses
{
    public class CommentResponse
    {
        public int CommentId { get; set; }
        public int? ResponseCommentId { get; set; }
        public UserSimplifiedResponse? RespondingUserResponse { get; set; }

        public UserSimplifiedResponse UserResponse { get; set; }

        public string Content { get; set; } = null!;
        public DateTime CreationDate { get; set; }

        public int PostIdNavigation { get; set; }
    }
}
