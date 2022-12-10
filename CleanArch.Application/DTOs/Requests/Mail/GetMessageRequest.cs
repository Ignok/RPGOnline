using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGOnline.Application.DTOs.Requests.Mail
{
    public class GetMessageRequest
    {
        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Please enter valid number")]
        public int Page { get; set; }
    }
}
