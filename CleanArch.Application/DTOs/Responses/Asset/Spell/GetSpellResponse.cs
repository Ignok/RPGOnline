using RPGOnline.Application.DTOs.Responses.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public string KeySkill { get; set; } = null!;
        public int MinValue { get; set; }
        public int ManaCost { get; set; }
        public string Effects { get; set; } = null!;
        public bool IsSaved { get; set; }
        public string PrefferedLanguage { get; set; } = null!;
        public virtual UserResponse CreatorNavigation { get; set; } = null!;
    }
}
