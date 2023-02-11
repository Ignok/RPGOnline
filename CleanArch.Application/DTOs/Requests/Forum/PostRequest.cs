using System.ComponentModel.DataAnnotations;

namespace RPGOnline.Application.DTOs.Requests.Forum
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

        [MaxLength(280)]
        public string? Picture { get; set; }
        public string? Tag { get; set; }
    }
}
