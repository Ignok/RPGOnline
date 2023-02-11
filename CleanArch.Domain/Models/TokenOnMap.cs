using System;
using System.Collections.Generic;

namespace RPGOnline.Domain.Models
{
    public partial class TokenOnMap
    {
        public int TokenOnMapId { get; set; }
        public int GameMapId { get; set; }
        public int GameParticipantId { get; set; }
        public int? VerticalPosition { get; set; }
        public int? HorizontalPosition { get; set; }

        public virtual GameMap GameMap { get; set; } = null!;
        public virtual GameParticipant GameParticipant { get; set; } = null!;
    }
}
