using System;
using System.Collections.Generic;

namespace RPGOnline.Domain.Models
{
    public partial class Friendship
    {
        public int UId { get; set; }
        public int FriendUId { get; set; }
        public int FriendshipStatus { get; set; }
        public int IsFollowed { get; set; }

        public virtual User FriendU { get; set; } = null!;
        public virtual User UIdNavigation { get; set; } = null!;
    }
}
