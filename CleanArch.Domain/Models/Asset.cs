using System;
using System.Collections.Generic;

namespace RPGOnline.Domain.Models
{
    public partial class Asset
    {
        public Asset()
        {
            Characters = new HashSet<Character>();
            Items = new HashSet<Item>();
            Professions = new HashSet<Profession>();
            Races = new HashSet<Race>();
            Spells = new HashSet<Spell>();
            UserSavedAssets = new HashSet<UserSavedAsset>();
        }

        public int AssetId { get; set; }
        public int AuthorId { get; set; }
        public bool IsPublic { get; set; }
        public string Language { get; set; } = null!;

        public virtual User Author { get; set; } = null!;
        public virtual ICollection<Character> Characters { get; set; }
        public virtual ICollection<Item> Items { get; set; }
        public virtual ICollection<Profession> Professions { get; set; }
        public virtual ICollection<Race> Races { get; set; }
        public virtual ICollection<Spell> Spells { get; set; }
        public virtual ICollection<UserSavedAsset> UserSavedAssets { get; set; }
    }
}
