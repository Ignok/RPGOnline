using System;
using System.Collections.Generic;

namespace RPGOnline.Domain.Models
{
    public partial class Trait
    {
        public Trait()
        {
            CharacterTraits = new HashSet<CharacterTrait>();
            TraitSkillModifications = new HashSet<TraitSkillModification>();
        }

        public int TraitId { get; set; }
        public int? AuthorUId { get; set; }
        public int IsRace { get; set; }
        public string TraitName { get; set; } = null!;
        public string Commentary { get; set; } = null!;
        public string Talent { get; set; } = null!;
        public string? HiddenTalent { get; set; }
        public int? KeySkill { get; set; }

        public virtual User? AuthorU { get; set; }
        public virtual ICollection<CharacterTrait> CharacterTraits { get; set; }
        public virtual ICollection<TraitSkillModification> TraitSkillModifications { get; set; }
    }
}
