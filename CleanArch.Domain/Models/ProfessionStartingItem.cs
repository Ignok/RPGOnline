using System;
using System.Collections.Generic;

namespace RPGOnline.Domain.Models
{
    public partial class ProfessionStartingItem
    {
        public int ProfessionStartingItemsId { get; set; }
        public int ProfessionId { get; set; }
        public int ItemId { get; set; }

        public virtual Item Item { get; set; } = null!;
        public virtual Profession Profession { get; set; } = null!;
    }
}
