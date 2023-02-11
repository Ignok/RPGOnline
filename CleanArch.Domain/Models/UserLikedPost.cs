using System;
using System.Collections.Generic;

namespace RPGOnline.Domain.Models
{
    public partial class UserLikedPost
    {
        public int UId { get; set; }
        public int PostId { get; set; }
        public DateTime LikeDate { get; set; }

        public virtual Post Post { get; set; } = null!;
        public virtual User UIdNavigation { get; set; } = null!;
    }
}
