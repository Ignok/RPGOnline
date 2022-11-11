using System;
using System.Collections.Generic;

namespace RPGOnline.Domain.Models
{
    public partial class UserAchievement
    {
        public int UId { get; set; }
        public int AchievementId { get; set; }
        public DateTime CompletionDate { get; set; }

        public virtual Achievement Achievement { get; set; } = null!;
        public virtual User UIdNavigation { get; set; } = null!;
    }
}
