using System.ComponentModel.DataAnnotations;

namespace RPGOnline.Application.DTOs.Requests
{
    public class UserRequest
    {
        public string? Country { get; set; }
        public string? City { get; set; }
        public string? AboutMe { get; set; }
        [Required]
        public string Attitude { get; set; } = null!;
    }
}
