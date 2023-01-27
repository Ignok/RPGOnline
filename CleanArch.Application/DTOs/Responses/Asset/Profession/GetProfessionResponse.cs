using RPGOnline.Application.DTOs.Responses.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGOnline.Application.DTOs.Responses.Asset.Profession
{
    public class GetProfessionResponse
    {
        public int AssetId { get; set; }
        public DateTime CreationDate { get; set; }
        public int TimesSaved { get; set; }
        public int ProfessionId { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Talent { get; set; } = null!;
        public string HiddenTalent { get; set; } = null!;
        public string? KeyAttribute { get; set; }
        public int WeaponMod { get; set; }
        public int ArmorMod { get; set; }
        public int GadgetMod { get; set; }
        public int CompanionMod { get; set; }
        public int PsycheMod { get; set; }
        public bool IsSaved { get; set; }
        public string PrefferedLanguage { get; set; } = null!;
        public virtual UserResponse CreatorNavigation { get; set; } = null!;
    }
}
