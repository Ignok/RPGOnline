using System;
using System.Collections.Generic;

namespace RPGOnline.Domain.Models
{
    public partial class Game
    {
        public Game()
        {
            GameMaps = new HashSet<GameMap>();
            GameParticipants = new HashSet<GameParticipant>();
            Notes = new HashSet<Note>();
        }

        public int GameId { get; set; }
        public int CreatorUId { get; set; }
        public string GameName { get; set; } = null!;
        public string Description { get; set; } = null!;
        public DateTime CreationDate { get; set; }
        public DateTime LastPlayed { get; set; }
        public int GameStatus { get; set; }

        public virtual User CreatorU { get; set; } = null!;
        public virtual ICollection<GameMap> GameMaps { get; set; }
        public virtual ICollection<GameParticipant> GameParticipants { get; set; }
        public virtual ICollection<Note> Notes { get; set; }
    }
}
