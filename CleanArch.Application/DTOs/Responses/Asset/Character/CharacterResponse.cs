using RPGOnline.Application.DTOs.Responses.Asset.Character.Character;
using RPGOnline.Application.DTOs.Responses.Asset.Profession;
using RPGOnline.Application.DTOs.Responses.Asset.Race;
using RPGOnline.Application.DTOs.Responses.User;

namespace RPGOnline.Application.DTOs.Responses.Asset.Character
{
    public class CharacterResponse
    {

        public int CharacterId { get; set; }
        public int AssetId { get; set; }
        public DateTime CreationDate { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public int Gold { get; set; }
        public string? Avatar { get; set; }

        public virtual FromJsonResponse JsonResponse { get; set; } = null!;

        public GetProfessionSimplifiedResponse? Profession { get; set; }
        public GetRaceSimplifiedResponse? Race { get; set; }

        public ICollection<CharacterItemResponse>? ItemList { get; set; }
        public ICollection<CharacterSpellResponse>? SpellList { get; set; }

        public virtual UserSimplifiedResponse CreatorNavigation { get; set; } = null!;

        public bool IsSaved { get; set; }
        public int TimesSaved { get; set; }
        public string PrefferedLanguage { get; set; } = null!;
    }
}
