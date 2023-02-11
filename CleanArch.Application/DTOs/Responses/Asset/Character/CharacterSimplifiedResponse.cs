using RPGOnline.Application.DTOs.Responses.Asset.Character.Character;
using RPGOnline.Application.DTOs.Responses.User;

namespace RPGOnline.Application.DTOs.Responses.Asset.Character
{
    public class CharacterSimplifiedResponse
    {
        public int CharacterId { get; set; }
        public int AssetId { get; set; }
        public DateTime CreationDate { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public int Gold { get; set; }
        public string? Avatar { get; set; }

        public virtual FromJsonResponse JsonResponse { get; set; } = null!;

        public string? ProfessionName { get; set; }
        public string? RaceName { get; set; }

        public ICollection<string>? ItemsNames { get; set; }
        public ICollection<string>? SpellsNames { get; set; }

        public virtual UserSimplifiedResponse CreatorNavigation { get; set; } = null!;
        public string PrefferedLanguage { get; set; } = null!;
    }
}
