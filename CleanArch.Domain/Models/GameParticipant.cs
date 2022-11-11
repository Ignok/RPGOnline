using System;
using System.Collections.Generic;

namespace RPGOnline.Domain.Models
{
    public partial class GameParticipant
    {
        public GameParticipant()
        {
            ChatMessages = new HashSet<ChatMessage>();
            GameParticipantItems = new HashSet<GameParticipantItem>();
            GameParticipantNotes = new HashSet<GameParticipantNote>();
            GameParticipantSpells = new HashSet<GameParticipantSpell>();
            TokenOnMaps = new HashSet<TokenOnMap>();
        }

        public int GameParticipantId { get; set; }
        public int GameId { get; set; }
        public int UId { get; set; }
        public int? CharacterId { get; set; }
        public int HealthPoints { get; set; }
        public int ManaPoints { get; set; }
        public int Colour { get; set; }
        public int Gold { get; set; }

        public virtual Character? Character { get; set; }
        public virtual Game Game { get; set; } = null!;
        public virtual User UIdNavigation { get; set; } = null!;
        public virtual ICollection<ChatMessage> ChatMessages { get; set; }
        public virtual ICollection<GameParticipantItem> GameParticipantItems { get; set; }
        public virtual ICollection<GameParticipantNote> GameParticipantNotes { get; set; }
        public virtual ICollection<GameParticipantSpell> GameParticipantSpells { get; set; }
        public virtual ICollection<TokenOnMap> TokenOnMaps { get; set; }
    }
}
