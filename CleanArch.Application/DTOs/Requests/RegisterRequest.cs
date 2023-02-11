using System.ComponentModel.DataAnnotations;

namespace RPGOnline.Application.DTOs.Requests
{
    public class RegisterRequest
    {
        [Required]
        [MaxLength(40)]
        public string Username { get; set; }

        [Required]
        [MaxLength(30)]
        public string Pswd { get; set; }

        [Required]
        [MaxLength(40)]
        public string Email { get; set; }
    }
}
