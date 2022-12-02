using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGOnline.Application.DTOs.Responses
{
    public class PutUserResponse
    {
        public string? Country { get; set; }
        public string? City { get; set; }
        public string? AboutMe { get; set; }
        public string Attitude { get; set; } = null!;
    }
}
