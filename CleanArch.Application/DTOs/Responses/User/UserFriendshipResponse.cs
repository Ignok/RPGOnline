using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGOnline.Application.DTOs.Responses.User
{
    public class UserFriendshipResponse
    {
        public int UId { get; set; }
        public string Username { get; set; } = null!;
        public int Picture { get; set; }
        public string? Country { get; set; }
        public string Attitude { get; set; } = null!;

        public bool IsFriend { get; set; }
        public bool IsFollowed { get; set; }
        public bool IsBlocked { get; set; }
        public bool IsRequestSent { get; set; }
        public bool IsRequestReceived { get; set; }
    }
}
