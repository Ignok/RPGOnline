using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
