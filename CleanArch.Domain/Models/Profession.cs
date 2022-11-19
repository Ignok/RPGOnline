using System;
using System.Collections.Generic;

namespace RPGOnline.Domain.Models
{
    public partial class Profession
    {
        public Profession()
        {
            Characters = new HashSet<Character>();
            ProfessionStartingItems = new HashSet<ProfessionStartingItem>();
            Spells = new HashSet<Spell>();
        }

        public int ProfessionId { get; set; }
        public int AssetId { get; set; }
        public string ProfessionName { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Talent { get; set; } = null!;
        public string HiddenTalent { get; set; } = null!;
        public string? KeyAttribute { get; set; }
        public int WeaponMod { get; set; }
        public int ArmorMod { get; set; }
        public int GadgetMod { get; set; }
        public int CompanionMod { get; set; }
        public int PsycheMod { get; set; }

        public virtual Asset Asset { get; set; } = null!;
        public virtual ICollection<Character> Characters { get; set; }
        public virtual ICollection<ProfessionStartingItem> ProfessionStartingItems { get; set; }

        public virtual ICollection<Spell> Spells { get; set; }
    }
}
