using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGOnline.Application.DTOs.Responses.Asset.Character.Character
{
    public class AttributesResponse
    {
        public int Strength { get; set; }
        public int Dexterity { get; set; }
        public int Intelligence { get; set; }
        public int Charisma { get; set; }
        public int Health { get; set; }
        public int Mana { get; set; }
    }
}
