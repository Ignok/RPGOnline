﻿using System;
using System.Collections.Generic;

namespace RPGOnline.Infrastructure.Models
{
    public partial class CharacterItem
    {
        public int CharacterItemsId { get; set; }
        public int ItemId { get; set; }
        public int CharacterId { get; set; }

        public virtual Character Character { get; set; } = null!;
        public virtual Item Item { get; set; } = null!;
    }
}
