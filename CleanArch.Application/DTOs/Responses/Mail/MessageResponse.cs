using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGOnline.Application.DTOs.Responses
{
    public class MessageResponse
    {
        public int MessageId { get; set; } 
        public int SenderId { get; set; }
        public int SenderPicture { get; set; }
        public string? SenderUsername { get; set; } 
        public string Title { get; set; } = null!;
        public string Content { get; set; } = null!;
        public DateTime SendDate { get; set; }
        public bool IsOpened { get; set; }
    }
}
