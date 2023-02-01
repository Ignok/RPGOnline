using RPGOnline.Application.DTOs.Responses.Asset.Character.Character;
using RPGOnline.Application.DTOs.Responses.Asset.Item;
using RPGOnline.Application.DTOs.Responses.Asset.Profession;
using RPGOnline.Application.DTOs.Responses.Asset.Race;
using RPGOnline.Application.DTOs.Responses.Asset.Spell;
using RPGOnline.Application.DTOs.Responses.User;
using RPGOnline.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGOnline.Application.DTOs.Responses.Character
{
    public class CharacterResponse
    {

        public int CharacterId { get; set; }
        public int AssetId { get; set; }
        public DateTime CreationDate { get; set; }
        public string CharacterName { get; set; } = null!;
        public string? Remarks { get; set; }
        public int Gold { get; set; }
        public string? Avatar { get; set; }

        public virtual FromJsonResponse JsonResponse { get; set; } = null!;

        public GetProfessionSimplifiedResponse? Profession { get; set; }
        public GetRaceSimplifiedResponse? Race { get; set; }

        public ICollection<CharacterItemResponse>? Items { get; set; }
        public ICollection<CharacterSpellResponse>? Spells { get; set; }

        //public bool IsSaved { get; set; }
        //public string PrefferedLanguage { get; set; } = null!;

        public virtual UserSimplifiedResponse CreatorNavigation { get; set; } = null!;

    }
}
