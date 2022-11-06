using System;
using System.Collections.Generic;

namespace RPGOnline.Infrastructure.Models
{
    public partial class TraitSkillModification
    {
        public int TraitId { get; set; }
        public int SkillFeatureId { get; set; }
        public int Modifier { get; set; }

        public virtual FeatureGlossary SkillFeature { get; set; } = null!;
        public virtual Trait Trait { get; set; } = null!;
    }
}
