using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGOnline.Application.DTOs.Responses.Character
{
    public class CharacteristicsResponse
    {
        public string Voice { get; set; } = null!;
        public string Posture { get; set; } = null!;
        public string Temperament { get; set; } = null!;
        public string Beliefs { get; set; } = null!;
        public string Face { get; set; } = null!;
        public string Origins { get; set; } = null!;
    }
}
