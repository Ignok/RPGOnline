using System;
using System.Collections.Generic;

namespace RPGOnline.Infrastructure.Models
{
    public partial class Post
    {
        public Post()
        {
            Comments = new HashSet<Comment>();
            UIds = new HashSet<User>();
        }

        public int PostId { get; set; }
        public int UId { get; set; }
        public string Title { get; set; } = null!;
        public string Content { get; set; } = null!;
        public string? Picture { get; set; }
        public DateTime CreationDate { get; set; }

        //number of likes
        public virtual ICollection<User> UIds { get; set; }

        public virtual User UIdNavigation { get; set; } = null!;
        public virtual ICollection<Comment> Comments { get; set; }

    }
}
