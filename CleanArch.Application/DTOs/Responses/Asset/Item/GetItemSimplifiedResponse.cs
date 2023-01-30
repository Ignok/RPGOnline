using RPGOnline.Application.DTOs.Responses.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGOnline.Application.DTOs.Responses.Asset.Item
{
    public class GetItemSimplifiedResponse
    {
        public int AssetId { get; set; }
        public int ItemId { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string KeySkill { get; set; } = null!;
        public int SkillMod { get; set; }
        public int GoldMultiplier { get; set; }
        public bool IsSaved { get; set; }
        public string PrefferedLanguage { get; set; } = null!;
    }
}
