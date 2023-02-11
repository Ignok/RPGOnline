using System;
using System.Collections.Generic;

namespace RPGOnline.Domain.Models
{
    public partial class Note
    {
        public Note()
        {
            GameParticipantNotes = new HashSet<GameParticipantNote>();
        }

        public int NoteId { get; set; }
        public int GameId { get; set; }
        public string Title { get; set; } = null!;
        public string? Content { get; set; }
        public string? Picture { get; set; }

        public virtual Game Game { get; set; } = null!;
        public virtual ICollection<GameParticipantNote> GameParticipantNotes { get; set; }
    }
}
