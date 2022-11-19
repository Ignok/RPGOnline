using System;
using System.Collections.Generic;

namespace RPGOnline.Domain.Models
{
    public partial class Race
    {
        public Race()
        {
            Characters = new HashSet<Character>();
        }

        public int RaceId { get; set; }
        public int AssetId { get; set; }
        public string RaceName { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Talent { get; set; } = null!;
        public string HiddenTalent { get; set; } = null!;
        public string? KeyAttribute { get; set; }

        public virtual Asset Asset { get; set; } = null!;
        public virtual ICollection<Character> Characters { get; set; }
    }
}
