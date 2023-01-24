using System;
using System.Collections.Generic;

namespace RPGOnline.Domain.Models
{
    public partial class Item
    {
        public Item()
        {
            CharacterItems = new HashSet<CharacterItem>();
            GameParticipantItems = new HashSet<GameParticipantItem>();
            ProfessionStartingItems = new HashSet<ProfessionStartingItem>();
        }

        public int ItemId { get; set; }
        public int AssetId { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string KeySkill { get; set; } = null!;
        public int SkillMod { get; set; }
        public int GoldMultiplier { get; set; }

        public virtual Asset Asset { get; set; } = null!;
        public virtual ICollection<CharacterItem> CharacterItems { get; set; }
        public virtual ICollection<GameParticipantItem> GameParticipantItems { get; set; }
        public virtual ICollection<ProfessionStartingItem> ProfessionStartingItems { get; set; }
    }
}
