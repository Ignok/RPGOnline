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
        public string ItemName { get; set; } = null!;
        public string ItemDescription { get; set; } = null!;
        public string ItemKeySkill { get; set; } = null!;
        public int ItemSkillMod { get; set; }
        public int ItemGoldMultiplier { get; set; }
    }
}
