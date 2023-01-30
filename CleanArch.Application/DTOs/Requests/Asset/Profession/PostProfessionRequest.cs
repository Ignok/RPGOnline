using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGOnline.Application.DTOs.Requests.Asset.Profession
{
    public class PostProfessionRequest
    {
        [Required]
        public int UId { get; set; }

        [Required]
        public bool IsPublic { get; set; }

        [Required]
        [StringLength(2)]
        public string Language { get; set; } = null!;

        [Required]
        [MinLength(1)]
        [MaxLength(80)]
        public string Name { get; set; } = null!;

        [Required]
        [MinLength(5)]
        [MaxLength(280)]
        public string Description { get; set; } = null!;

        [Required]
        [MinLength(5)]
        [MaxLength(280)]
        public string Talent { get; set; } = null!;

        [Required]
        [MinLength(5)]
        [MaxLength(280)]
        public string HiddenTalent { get; set; } = null!;

        [MaxLength(40)]
        public string? KeyAttribute { get; set; }

        [Required]
        [Range(-6, 6)]
        public int WeaponMod { get; set; }

        [Required]
        [Range(-6, 6)]
        public int ArmorMod { get; set; }

        [Required]
        [Range(-6, 6)]
        public int GadgetMod { get; set; }

        [Required]
        [Range(-6, 6)]
        public int CompanionMod { get; set; }

        [Required]
        [Range(-6, 6)]
        public int PsycheMod { get; set; }

        public int SpellId { get; set; }

        public int ItemId { get; set; }
    }
}
