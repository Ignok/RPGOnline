using System;
using System.Collections.Generic;

namespace RPGOnline.Infrastructure.Models
{
    public partial class CharacterSkill
    {
        public int FeatureId { get; set; }
        public int CharacterId { get; set; }
        public int Quantity { get; set; }

        public virtual Character Character { get; set; } = null!;
        public virtual FeatureGlossary Feature { get; set; } = null!;
    }
}
