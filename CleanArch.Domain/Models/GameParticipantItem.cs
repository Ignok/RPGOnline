using System;
using System.Collections.Generic;

namespace RPGOnline.Domain.Models
{
    public partial class GameParticipantItem
    {
        public int GameParticipantItemsId { get; set; }
        public int GameParticipantId { get; set; }
        public int ItemId { get; set; }

        public virtual GameParticipant GameParticipant { get; set; } = null!;
        public virtual Item Item { get; set; } = null!;
    }
}
