using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGOnline.Application.DTOs.Requests
{
    public class PostRequest
    {
        [Required]
        public int UId { get; set; }

        [Required]
        [MaxLength(40)]
        public string Title { get; set; }
        
        [Required]
        [MaxLength(1080)]
        public string Content { get; set; }

        
        public string? Picture { get; set; }
        public string? Tag { get; set; }
    }
}
