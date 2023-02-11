using RPGOnline.Application.DTOs.Responses.Asset.Item;
using RPGOnline.Application.DTOs.Responses.Asset.Spell;
using RPGOnline.Application.DTOs.Responses.User;

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
        public virtual UserSimplifiedResponse CreatorNavigation { get; set; } = null!;

        public ICollection<GetSpellSimplifiedResponse>? SpellList { get; set; }
        public ICollection<GetItemSimplifiedResponse>? ItemList { get; set; }
    }
}
