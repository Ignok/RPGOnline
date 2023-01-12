using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGOnline.Application.DTOs.Responses.Asset.Spell
{
    public class GetSpellSimplifiedResponse
    {
        public int AssetId { get; set; }
        public int SpellId { get; set; }
        public string SpellName { get; set; } = null!;
        public string SpellDescription { get; set; } = null!;
        public string SpellKeySkill { get; set; } = null!;
        public int SpellMinValue { get; set; }
        public int SpellManaCost { get; set; }
    }
}
