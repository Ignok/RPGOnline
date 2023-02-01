using RPGOnline.Application.DTOs.Responses.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGOnline.Application.DTOs.Responses.Asset.Character.Character
{
    public class CharacterItemResponse
    {
        public int AssetId { get; set; }
        public int ItemId { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string KeySkill { get; set; } = null!;
        public int SkillMod { get; set; }
        public int GoldMultiplier { get; set; }
        public int Quantity { get; set; }
        public bool IsSaved { get; set; }
        public string PrefferedLanguage { get; set; } = null!;
        public bool IsPublic { get; set; }
        public int AuthorId { get; set; }
        public string AuthorUsername { get; set; } = null!;
    }
}
