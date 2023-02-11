using RPGOnline.Application.DTOs.Responses.User;

namespace RPGOnline.Application.DTOs.Responses.Asset.Spell
{
    public class GetSpellSimplifiedResponse
    {
        public int AssetId { get; set; }
        public int SpellId { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string KeyAttribute { get; set; } = null!;
        public int MinValue { get; set; }
        public int ManaCost { get; set; }
        public string Effects { get; set; } = null!;
        public bool IsSaved { get; set; }
        public string PrefferedLanguage { get; set; } = null!;
        public virtual UserSimplifiedResponse CreatorNavigation { get; set; } = null!;
        public bool IsPublic { get; set; }
    }
}
