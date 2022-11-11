using System;
using System.Collections.Generic;

namespace RPGOnline.Domain.Models
{
    public partial class ItemSkillModification
    {
        public int SkillFeatureId { get; set; }
        public int ItemId { get; set; }
        public int Modifier { get; set; }

        public virtual Item Item { get; set; } = null!;
        public virtual FeatureGlossary SkillFeature { get; set; } = null!;
    }
}
