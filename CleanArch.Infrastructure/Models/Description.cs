using System;
using System.Collections.Generic;

namespace RPGOnline.Infrastructure.Models
{
    public partial class Description
    {
        public Description()
        {
            CharacterDescriptions = new HashSet<CharacterDescription>();
        }

        public int DescriptionId { get; set; }
        public int DescriptionFeatureId { get; set; }
        public string Content { get; set; } = null!;

        public virtual FeatureGlossary DescriptionFeature { get; set; } = null!;
        public virtual ICollection<CharacterDescription> CharacterDescriptions { get; set; }
    }
}
