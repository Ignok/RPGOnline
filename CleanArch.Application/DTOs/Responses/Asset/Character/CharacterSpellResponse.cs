namespace RPGOnline.Application.DTOs.Responses.Asset.Character
{
    public class CharacterSpellResponse
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
        public bool IsPublic { get; set; }
        public int AuthorId { get; set; }
        public string AuthorUsername { get; set; } = null!;
    }
}
