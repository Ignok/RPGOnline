﻿namespace RPGOnline.Application.DTOs.Responses.Friendship
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
        public byte MyRating { get; set; } = 0;
        public double AverageRating { get; set; } = 0.0;
    }
}
