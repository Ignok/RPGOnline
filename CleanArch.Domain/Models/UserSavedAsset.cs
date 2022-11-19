using System;
using System.Collections.Generic;

namespace RPGOnline.Domain.Models
{
    public partial class UserSavedAsset
    {
        public int UId { get; set; }
        public int AssetId { get; set; }
        public DateTime SaveDate { get; set; }

        public virtual Asset Asset { get; set; } = null!;
        public virtual User UIdNavigation { get; set; } = null!;
    }
}
