using RPGOnline.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGOnline.Application.DTOs.Requests.User
{
    public class SearchUserRequest
    {
        public int Rating { get; set; }
        public string? Attitude { get; set; }
        public string? Search { get; set; }
    }
}
