using System;
using System.Collections.Generic;

namespace RPGOnline.Domain.Models
{
    public partial class CharacterItem
    {
        public int CharacterItemsId { get; set; }
        public int ItemId { get; set; }
        public int CharacterId { get; set; }
        public int Quantity { get; set; }

        public virtual Character Character { get; set; } = null!;
        public virtual Item Item { get; set; } = null!;
    }
}
