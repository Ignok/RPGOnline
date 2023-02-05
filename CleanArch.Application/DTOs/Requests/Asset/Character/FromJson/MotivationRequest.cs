using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGOnline.Application.DTOs.Requests.Asset.Character.FromJson
{
    public class MotivationRequest
    {
        [Required]
        [MinLength(1)]
        [MaxLength(40)]
        public string Objective { get; set; } = null!;

        [Required]
        [MinLength(1)]
        [MaxLength(40)]
        public string Subject { get; set; } = null!;

        [Required]
        [MinLength(1)]
        [MaxLength(40)]
        public string What_Happened { get; set; } = null!;

        [Required]
        [MinLength(1)]
        [MaxLength(40)]
        public string Where_Happened { get; set; } = null!;

        [Required]
        [MinLength(1)]
        [MaxLength(40)]
        public string How_Happened { get; set; } = null!;

        [Required]
        [MinLength(1)]
        [MaxLength(40)]
        public string Destination { get; set; } = null!;
    }
}
