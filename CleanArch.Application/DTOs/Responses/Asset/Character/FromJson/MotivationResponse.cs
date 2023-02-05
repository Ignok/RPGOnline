using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGOnline.Application.DTOs.Responses.Character
{
    public class MotivationResponse
    {
        public string Objective { get; set; } = null!;
        public string Subject { get; set; } = null!;
        public string What_Happened { get; set; } = null!;
        public string Where_Happened { get; set; } = null!;
        public string How_Happened { get; set; } = null!;
        public string Destination { get; set; } = null!;
    }
}
