using System;
using System.Collections.Generic;

namespace RPGOnline.Domain.Models
{
    public partial class CharacterTrait
    {
        public int CharacterTraitsId { get; set; }
        public int CharacterId { get; set; }
        public int TraitId { get; set; }

        public virtual Character Character { get; set; } = null!;
        public virtual Trait Trait { get; set; } = null!;
    }
}
