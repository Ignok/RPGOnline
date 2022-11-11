using System;
using System.Collections.Generic;

namespace RPGOnline.Domain.Models
{
    public partial class CharacterDescription
    {
        public int CharacterDescriptionsId { get; set; }
        public int CharacterId { get; set; }
        public int DescriptionId { get; set; }

        public virtual Character Character { get; set; } = null!;
        public virtual Description Description { get; set; } = null!;
    }
}
