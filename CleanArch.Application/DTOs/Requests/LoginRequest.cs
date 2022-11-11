using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGOnline.Application.DTOs.Requests
{
    public class LoginRequest
    {
        [Required]
        [MaxLength(40)]
        public string Login { get; set; }

        [Required]
        [MaxLength(30)]
        public string Password { get; set; }
    }
}
