using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGOnline.Application.DTOs.Responses.Friendship
{
    public class FriendshipResponse
    {
        public bool IsFriend { get; set; }
        public bool IsFollowed { get; set; }
        public bool IsBlocked { get; set; }
        public bool IsRequestSent { get; set; }
        public bool IsRequestReceived { get; set; }
        public bool HasBlockedMe { get; set; }
    }
}
