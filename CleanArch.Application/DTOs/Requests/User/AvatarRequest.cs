using System.ComponentModel.DataAnnotations;

namespace RPGOnline.Application.DTOs.Requests.User
{
    public class AvatarRequest
    {
        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Please enter valid number")]
        public int Picture { get; set; } = 0;
    }
}
