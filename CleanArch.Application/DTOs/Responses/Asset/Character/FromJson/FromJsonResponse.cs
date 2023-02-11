using RPGOnline.Application.DTOs.Responses.Character;

namespace RPGOnline.Application.DTOs.Responses.Asset.Character.Character
{
    public class FromJsonResponse
    {
        public virtual MotivationResponse? Motivation { get; set; }
        public virtual CharacteristicsResponse? Characteristics { get; set; }
        public virtual SkillsetResponse Skillset { get; set; } = null!;
        public virtual AttributesResponse Attributes { get; set; } = null!;
    }
}
