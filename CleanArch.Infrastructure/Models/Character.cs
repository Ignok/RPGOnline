using System;
using System.Collections.Generic;

namespace RPGOnline.Infrastructure.Models
{
    public partial class Character
    {
        public Character()
        {
            CharacterDescriptions = new HashSet<CharacterDescription>();
            CharacterItems = new HashSet<CharacterItem>();
            CharacterSkills = new HashSet<CharacterSkill>();
            CharacterSpells = new HashSet<CharacterSpell>();
            CharacterTraits = new HashSet<CharacterTrait>();
            GameParticipants = new HashSet<GameParticipant>();
        }

        public int CharacterId { get; set; }
        public int? UId { get; set; }
        public int AvatarId { get; set; }
        public string CharacterName { get; set; } = null!;
        public string? Remarks { get; set; }
        public int IsPublic { get; set; }
        public int Gold { get; set; }

        public virtual Avatar Avatar { get; set; } = null!;
        public virtual User? UIdNavigation { get; set; }
        public virtual ICollection<CharacterDescription> CharacterDescriptions { get; set; }
        public virtual ICollection<CharacterItem> CharacterItems { get; set; }
        public virtual ICollection<CharacterSkill> CharacterSkills { get; set; }
        public virtual ICollection<CharacterSpell> CharacterSpells { get; set; }
        public virtual ICollection<CharacterTrait> CharacterTraits { get; set; }
        public virtual ICollection<GameParticipant> GameParticipants { get; set; }
    }
}
