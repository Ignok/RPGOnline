using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGOnline.Application.DTOs.Responses.Asset.Character.Character
{
    public class SkillsetResponse
    {
        public int Weapon { get; set; }
        public int Armor { get; set; }
        public int Gadget { get; set; }
        public int Companion { get; set; }
        public int Psyche { get; set; }
    }
}