using System;
using System.Collections.Generic;

namespace RPGOnline.Domain.Models
{
    public partial class Characteristic
    {
        public int CharacteristicsId { get; set; }
        public string Voice { get; set; } = null!;
        public string Posture { get; set; } = null!;
        public string Temperament { get; set; } = null!;
        public string Beliefs { get; set; } = null!;
        public string Face { get; set; } = null!;
        public string Origins { get; set; } = null!;
    }
}
