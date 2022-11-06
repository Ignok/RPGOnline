using System;
using System.Collections.Generic;

namespace RPGOnline.Infrastructure.Models
{
    public partial class GameParticipantSpell
    {
        public int GameParticipantSpellsId { get; set; }
        public int GameParticipantId { get; set; }
        public int SpellId { get; set; }

        public virtual GameParticipant GameParticipant { get; set; } = null!;
        public virtual Spell Spell { get; set; } = null!;
    }
}
