using System;
using System.Collections.Generic;

namespace RPGOnline.Infrastructure.Models
{
    public partial class Item
    {
        public Item()
        {
            CharacterItems = new HashSet<CharacterItem>();
            GameParticipantItems = new HashSet<GameParticipantItem>();
            ItemSkillModifications = new HashSet<ItemSkillModification>();
        }

        public int ItemId { get; set; }
        public int? AuthorUId { get; set; }
        public string ItemName { get; set; } = null!;
        public string Commentary { get; set; } = null!;
        public int KeySkill { get; set; }
        public int MinValue { get; set; }
        public int GoldMultiplier { get; set; }

        public virtual User? AuthorU { get; set; }
        public virtual ICollection<CharacterItem> CharacterItems { get; set; }
        public virtual ICollection<GameParticipantItem> GameParticipantItems { get; set; }
        public virtual ICollection<ItemSkillModification> ItemSkillModifications { get; set; }
    }
}
