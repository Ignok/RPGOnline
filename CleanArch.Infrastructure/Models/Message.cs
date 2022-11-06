using System;
using System.Collections.Generic;

namespace RPGOnline.Infrastructure.Models
{
    public partial class Message
    {
        public int MessageId { get; set; }
        public int SenderUId { get; set; }
        public int ReceiverUId { get; set; }
        public string Title { get; set; } = null!;
        public string Content { get; set; } = null!;
        public DateTime SendDate { get; set; }

        public virtual User ReceiverU { get; set; } = null!;
        public virtual User SenderU { get; set; } = null!;
    }
}
