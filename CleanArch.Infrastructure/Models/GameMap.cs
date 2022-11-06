using System;
using System.Collections.Generic;

namespace RPGOnline.Infrastructure.Models
{
    public partial class GameMap
    {
        public GameMap()
        {
            TokenOnMaps = new HashSet<TokenOnMap>();
        }

        public int GameMapsId { get; set; }
        public int MapId { get; set; }
        public int GameId { get; set; }
        public int VerticalBox { get; set; }
        public int HorizontalBox { get; set; }

        public virtual Game Game { get; set; } = null!;
        public virtual Map Map { get; set; } = null!;
        public virtual ICollection<TokenOnMap> TokenOnMaps { get; set; }
    }
}
