using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGOnline.Application.DTOs.Responses.Character
{
    public class RaceResponse
    {
        public int RaceId { get; set; }
        public int AssetId { get; set; }
        public string RaceName { get; set; }=null!;
        public string Description { get; set; } = null!;
        public string Talent { get; set; } = null!;
        public string HiddenTalent { get; set; } = null!;
        public string? KeyAttribute { get; set; }
    }
}
