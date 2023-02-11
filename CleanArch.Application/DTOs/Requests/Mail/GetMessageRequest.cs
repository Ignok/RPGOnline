using System.ComponentModel.DataAnnotations;

namespace RPGOnline.Application.DTOs.Requests.Mail
{
    public class GetMessageRequest
    {
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Please enter valid number")]
        public int Page { get; set; }
    }
}
