﻿namespace RPGOnline.Application.DTOs.Responses.Friendship
{
    public class FriendshipResponse
    {
        public bool IsFriend { get; set; }
        public bool IsFollowed { get; set; }
        public bool IsBlocked { get; set; }
        public bool IsRequestSent { get; set; }
        public bool IsRequestReceived { get; set; }
        public bool HasBlockedMe { get; set; }
        public byte MyRating { get; set; } = 0;
        public double AverageRating { get; set; } = 0.0;
    }
}
