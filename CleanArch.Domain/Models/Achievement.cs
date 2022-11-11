using System;
using System.Collections.Generic;

namespace RPGOnline.Domain.Models
{
    public partial class Achievement
    {
        public Achievement()
        {
            UserAchievements = new HashSet<UserAchievement>();
        }

        public int AchievementId { get; set; }
        public string? Picture { get; set; }
        public string AchievementName { get; set; } = null!;
        public string Commentary { get; set; } = null!;

        public virtual ICollection<UserAchievement> UserAchievements { get; set; }
    }
}
