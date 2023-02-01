using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGOnline.Application.DTOs.Responses.Character
{
    public class CharacterResponse
    {
        public virtual MotivationResponse Motivation { get; set; } = null!;
        public virtual CharacteristicsResponse Characteristics { get; set; } = null!;
    }
}
