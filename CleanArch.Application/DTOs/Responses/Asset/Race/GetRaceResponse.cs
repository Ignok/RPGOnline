using RPGOnline.Application.DTOs.Responses.User;

namespace RPGOnline.Application.DTOs.Responses.Asset.Race
{
    public class GetRaceResponse
    {
        public int AssetId { get; set; }
        public DateTime CreationDate { get; set; }
        public int TimesSaved { get; set; }
        public int RaceId { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Talent { get; set; } = null!;
        public string HiddenTalent { get; set; } = null!;
        public string? KeyAttribute { get; set; }
        public bool IsSaved { get; set; }
        public string PrefferedLanguage { get; set; } = null!;
        public virtual UserSimplifiedResponse CreatorNavigation { get; set; } = null!;
    }
}
