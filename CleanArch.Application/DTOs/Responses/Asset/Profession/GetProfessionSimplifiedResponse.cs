using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGOnline.Application.DTOs.Responses.Asset.Profession
{
    public class GetProfessionSimplifiedResponse
    {
        public int AssetId { get; set; }
        public int ProfessionId { get; set; }
        public string ProfessionName { get; set; } = null!;
        public string ProfessionDescription { get; set; } = null!;
        public string ProfessionTalent { get; set; } = null!;
        public string ProfessionHiddenTalent { get; set; } = null!;
        public string? ProfessionKeyAttribute { get; set; }
        public int WeaponMod { get; set; }
        public int ArmorMod { get; set; }
        public int GadgetMod { get; set; }
        public int CompanionMod { get; set; }
        public int PsycheMod { get; set; }
    }
}
