using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGOnline.Application.DTOs.Responses.User
{
    public class UserAboutmeResponse
    {
        public int UId { get; set; }
        public string Username { get; set; } = null!;
        public int Picture { get; set; }
        public string Email { get; set; } = null!;
        public string? Country { get; set; }
        public string? City { get; set; }
        public string? AboutMe { get; set; }
        public string Attitude { get; set; } = null!;
        public DateTime CreationDate { get; set; }
    }
}
