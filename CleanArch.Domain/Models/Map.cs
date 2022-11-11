using System;
using System.Collections.Generic;

namespace RPGOnline.Domain.Models
{
    public partial class Map
    {
        public Map()
        {
            GameMaps = new HashSet<GameMap>();
        }

        public int MapId { get; set; }
        public int? AuthorUId { get; set; }
        public string MapName { get; set; } = null!;
        public string Commentary { get; set; } = null!;
        public string? Picture { get; set; }
        public int IsPublic { get; set; }

        public virtual User? AuthorU { get; set; }
        public virtual ICollection<GameMap> GameMaps { get; set; }
    }
}
