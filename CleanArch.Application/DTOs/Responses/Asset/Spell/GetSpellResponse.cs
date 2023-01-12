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
        public string SpellName { get; set; } = null!;
        public string SpellDescription { get; set; } = null!;
        public string SpellKeySkill { get; set; } = null!;
        public int SpellMinValue { get; set; }
        public int SpellManaCost { get; set; }
        public string PrefferedLanguage { get; set; } = null!;
        public virtual UserResponse CreatorNavigation { get; set; } = null!;
    }
}
