using System;
using System.Collections.Generic;

namespace RPGOnline.Domain.Models
{
    public partial class User
    {
        public User()
        {
            Assets = new HashSet<Asset>();
            Comments = new HashSet<Comment>();
            FriendshipFriendUs = new HashSet<Friendship>();
            FriendshipUIdNavigations = new HashSet<Friendship>();
            GameParticipants = new HashSet<GameParticipant>();
            Games = new HashSet<Game>();
            Maps = new HashSet<Map>();
            MessageReceiverUs = new HashSet<Message>();
            MessageSenderUs = new HashSet<Message>();
            Posts = new HashSet<Post>();
            UserAchievements = new HashSet<UserAchievement>();
            UserLikedPosts = new HashSet<UserLikedPost>();
            UserSavedAssets = new HashSet<UserSavedAsset>();
        }

        public int UId { get; set; }
        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;
        public DateTime CreationDate { get; set; }
        public string? Country { get; set; }
        public string? City { get; set; }
        public string? AboutMe { get; set; }
        public string Attitude { get; set; } = null!;
        public string? Picture { get; set; }
        public string Pswd { get; set; } = null!;
        public string Salt { get; set; } = null!;
        public string RefreshToken { get; set; } = null!;
        public DateTime? RefreshTokenExp { get; set; }

        public virtual ICollection<Asset> Assets { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Friendship> FriendshipFriendUs { get; set; }
        public virtual ICollection<Friendship> FriendshipUIdNavigations { get; set; }
        public virtual ICollection<GameParticipant> GameParticipants { get; set; }
        public virtual ICollection<Game> Games { get; set; }
        public virtual ICollection<Map> Maps { get; set; }
        public virtual ICollection<Message> MessageReceiverUs { get; set; }
        public virtual ICollection<Message> MessageSenderUs { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
        public virtual ICollection<UserAchievement> UserAchievements { get; set; }
        public virtual ICollection<UserLikedPost> UserLikedPosts { get; set; }
        public virtual ICollection<UserSavedAsset> UserSavedAssets { get; set; }
    }
}
