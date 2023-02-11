using RPGOnline.Application.DTOs.Responses.User;

namespace RPGOnline.Application.DTOs.Responses.Asset.Item
{
    public class GetItemResponse
    {
        public int AssetId { get; set; }
        public DateTime CreationDate { get; set; }
        public int TimesSaved { get; set; }
        public int ItemId { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string KeySkill { get; set; } = null!;
        public int SkillMod { get; set; }
        public int GoldMultiplier { get; set; }
        public bool IsSaved { get; set; }
        public string PrefferedLanguage { get; set; } = null!;
        public virtual UserSimplifiedResponse CreatorNavigation { get; set; } = null!;
    }
}
