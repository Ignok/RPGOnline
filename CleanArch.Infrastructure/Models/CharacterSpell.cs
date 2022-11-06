using System;
using System.Collections.Generic;

namespace RPGOnline.Infrastructure.Models
{
    public partial class CharacterSpell
    {
        public int CharacterSpellsId { get; set; }
        public int SpellId { get; set; }
        public int CharacterId { get; set; }

        public virtual Character Character { get; set; } = null!;
        public virtual Spell Spell { get; set; } = null!;
    }
}
