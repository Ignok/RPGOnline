using RPGOnline.Application.DTOs.Responses.Character;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
