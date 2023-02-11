using System;
using System.Collections.Generic;

namespace RPGOnline.Domain.Models
{
    public partial class ChatMessage
    {
        public int MessageId { get; set; }
        public int GameParticipantId { get; set; }
        public string Content { get; set; } = null!;

        public virtual GameParticipant GameParticipant { get; set; } = null!;
    }
}
