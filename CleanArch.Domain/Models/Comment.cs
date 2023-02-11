using System;
using System.Collections.Generic;

namespace RPGOnline.Domain.Models
{
    public partial class Comment
    {
        public Comment()
        {
            InverseResponseComment = new HashSet<Comment>();
        }

        public int CommentId { get; set; }
        public int UId { get; set; }
        public int PostId { get; set; }
        public string Content { get; set; } = null!;
        public int? ResponseCommentId { get; set; }
        public DateTime CreationDate { get; set; }

        public virtual Post Post { get; set; } = null!;
        public virtual Comment? ResponseComment { get; set; }
        public virtual User UIdNavigation { get; set; } = null!;
        public virtual ICollection<Comment> InverseResponseComment { get; set; }
    }
}
