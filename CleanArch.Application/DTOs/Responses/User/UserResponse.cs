using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGOnline.Application.DTOs.Responses.User
{
    public class UserResponse
    {
        public int UId { get; set; }
        public string Username { get; set; } = null!;
        public int Picture { get; set; }
        public string? AboutMe { get; set; }
        public string Attitude { get; set; } = null!;
        public bool HasBlockedMe { get; set; } = false;
        public double AverageRating { get; set; } = 0.0;
    }
}
