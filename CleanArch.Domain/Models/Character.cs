using System;
using System.Collections.Generic;

namespace RPGOnline.Domain.Models
{
    public partial class Character
    {
        public Character()
        {
            CharacterItems = new HashSet<CharacterItem>();
            GameParticipants = new HashSet<GameParticipant>();
            Spells = new HashSet<Spell>();
        }

        public int CharacterId { get; set; }
        public int AssetId { get; set; }
        public int? RaceId { get; set; }
        public int? ProfessionId { get; set; }
        public string CharacterName { get; set; } = null!;
        public string? Remarks { get; set; }
        public int Gold { get; set; }
        public string? Avatar { get; set; }
        public string MotivationJson { get; set; } = null!;
        public string CharacteristicsJson { get; set; } = null!;
        public string SkillsetJson { get; set; } = null!;
        public string ProficiencyJson { get; set; } = null!;

        public virtual Asset Asset { get; set; } = null!;
        public virtual Profession? Profession { get; set; }
        public virtual Race? Race { get; set; }
        public virtual ICollection<CharacterItem> CharacterItems { get; set; }
        public virtual ICollection<GameParticipant> GameParticipants { get; set; }

        public virtual ICollection<Spell> Spells { get; set; }
    }
}
