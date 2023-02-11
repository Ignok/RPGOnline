namespace RPGOnline.Application.DTOs.Requests.Asset.Character.FromJson
{
    public class FromJsonRequest
    {
        public virtual MotivationRequest? Motivation { get;set; }
        public virtual CharacteristicsRequest? Characteristics { get; set; }
        public virtual SkillsetRequest? Skillset { get; set; } = null!;
        public virtual AttributesRequest Attributes { get; set; } = null!;
    }
}
