using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGOnline.Application.DTOs.Responses.Asset.Race
{
    public class GetRaceSimplifiedResponse
    {
        public int RaceId { get; set; }
        public int AssetId { get; set; }
        public string Name { get; set; }=null!;
        public string Description { get; set; } = null!;
        public string Talent { get; set; } = null!;
        public string HiddenTalent { get; set; } = null!;
        public string? KeyAttribute { get; set; }
        public string Language { get; set; } = null!;
    }
}
