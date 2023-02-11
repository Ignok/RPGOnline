using System;
using System.Collections.Generic;

namespace RPGOnline.Domain.Models
{
    public partial class GameParticipantNote
    {
        public int GameParticipantNotesId { get; set; }
        public int NoteId { get; set; }
        public int? GameParticipantId { get; set; }
        public string? FolderName { get; set; }

        public virtual GameParticipant? GameParticipant { get; set; }
        public virtual Note Note { get; set; } = null!;
    }
}
