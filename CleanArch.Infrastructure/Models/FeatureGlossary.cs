using System;
using System.Collections.Generic;

namespace RPGOnline.Infrastructure.Models
{
    public partial class FeatureGlossary
    {
        public FeatureGlossary()
        {
            CharacterSkills = new HashSet<CharacterSkill>();
            Descriptions = new HashSet<Description>();
            ItemSkillModifications = new HashSet<ItemSkillModification>();
            TraitSkillModifications = new HashSet<TraitSkillModification>();
        }

        public int FeatureId { get; set; }
        public string FeatureGlossaryName { get; set; } = null!;

        public virtual ICollection<CharacterSkill> CharacterSkills { get; set; }
        public virtual ICollection<Description> Descriptions { get; set; }
        public virtual ICollection<ItemSkillModification> ItemSkillModifications { get; set; }
        public virtual ICollection<TraitSkillModification> TraitSkillModifications { get; set; }
    }
}
