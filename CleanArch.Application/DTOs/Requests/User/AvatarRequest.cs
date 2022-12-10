using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGOnline.Application.DTOs.Requests.User
{
    public class AvatarRequest
    {
        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Please enter valid number")]
        public int Picture { get; set; } = 0;
    }
}
