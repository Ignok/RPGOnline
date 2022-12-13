using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGOnline.Application.DTOs.Requests.Character
{
    public class GetRaceRequest
    {
        public string Attribute { get; set; } = null!;
        public string[] PrefferedLanguage { get; set; } = null!;
    }
}
