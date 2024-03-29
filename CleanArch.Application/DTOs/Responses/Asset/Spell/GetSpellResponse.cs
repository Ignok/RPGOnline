﻿using RPGOnline.Application.DTOs.Responses.User;

namespace RPGOnline.Application.DTOs.Responses.Asset.Spell
{
    public class GetSpellResponse
    {
        public int AssetId { get; set; }
        public DateTime CreationDate { get; set; }
        public int TimesSaved { get; set; }
        public int SpellId { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string KeyAttribute { get; set; } = null!;
        public int MinValue { get; set; }
        public int ManaCost { get; set; }
        public string Effects { get; set; } = null!;
        public bool IsSaved { get; set; }
        public string PrefferedLanguage { get; set; } = null!;
        public virtual UserSimplifiedResponse CreatorNavigation { get; set; } = null!;
    }
}
