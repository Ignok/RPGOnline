using System;
using System.Collections.Generic;

namespace RPGOnline.Domain.Models
{
    public partial class Spell
    {
        public Spell()
        {
            CharacterSpells = new HashSet<CharacterSpell>();
            GameParticipantSpells = new HashSet<GameParticipantSpell>();
        }

        public int SpellId { get; set; }
        public int? AuthorUId { get; set; }
        public string SpellName { get; set; } = null!;
        public string Commentary { get; set; } = null!;
        public int KeySkill { get; set; }
        public int RequiredValue { get; set; }
        public int ManaCost { get; set; }
        public string Effects { get; set; } = null!;

        public virtual User? AuthorU { get; set; }
        public virtual ICollection<CharacterSpell> CharacterSpells { get; set; }
        public virtual ICollection<GameParticipantSpell> GameParticipantSpells { get; set; }
    }
}
