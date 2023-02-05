using RPGOnline.Application.DTOs.Responses.Asset.Character.Character;
using RPGOnline.Application.DTOs.Responses.Character;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
