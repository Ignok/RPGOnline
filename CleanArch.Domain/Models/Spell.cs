using System;
using System.Collections.Generic;

namespace RPGOnline.Domain.Models
{
    public partial class Spell
    {
        public Spell()
        {
            Characters = new HashSet<Character>();
            GameParticipants = new HashSet<GameParticipant>();
            Professions = new HashSet<Profession>();
        }

        public int SpellId { get; set; }
        public int AssetId { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string KeySkill { get; set; } = null!;
        public int MinValue { get; set; }
        public int ManaCost { get; set; }
        public string Effects { get; set; } = null!;

        public virtual Asset Asset { get; set; } = null!;

        public virtual ICollection<Character> Characters { get; set; }
        public virtual ICollection<GameParticipant> GameParticipants { get; set; }
        public virtual ICollection<Profession> Professions { get; set; }
    }
}
