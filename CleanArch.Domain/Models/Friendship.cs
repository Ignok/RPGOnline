using System;
using System.Collections.Generic;

namespace RPGOnline.Domain.Models
{
    public partial class Friendship
    {
        public int UId { get; set; }
        public int FriendUId { get; set; }
        public bool IsFriend { get; set; }
        public bool IsFollowed { get; set; }
        public bool IsBlocked { get; set; }
        public bool IsRequestSent { get; set; }
        public bool IsRequestReceived { get; set; }

        public virtual User FriendU { get; set; } = null!;
        public virtual User UIdNavigation { get; set; } = null!;
    }
}
