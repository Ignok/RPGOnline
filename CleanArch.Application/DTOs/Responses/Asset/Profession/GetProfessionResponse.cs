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
        public string ProfesionName { get; set; } = null!;
        public string ProfessionDescription { get; set; } = null!;
        public string ProfessionTalent { get; set; } = null!;
        public string ProfessionHiddenTalent { get; set; } = null!;
        public string? ProfessionKeyAttribute { get; set; }
        public int WeaponMod { get; set; }
        public int ArmorMod { get; set; }
        public int GadgetMod { get; set; }
        public int CompanionMod { get; set; }
        public int PsycheMod { get; set; }
        public string PrefferedLanguage { get; set; } = null!;
        public virtual UserResponse CreatorNavigation { get; set; } = null!;
    }
}
