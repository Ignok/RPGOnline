using System.ComponentModel.DataAnnotations;

namespace RPGOnline.Application.DTOs.Requests
{
    public class PostMessageRequest
    {
        [Required]
        [MaxLength(40)]
        public string Title { get; set; } = null!;

        [Required]
        [MaxLength(280)]
        public string Content { get; set; } = null!;

        [Required]
        [MaxLength(40)]
        public string ReceiverUsername { get; set; } = null!;
    }
}
