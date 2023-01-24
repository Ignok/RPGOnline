using System.ComponentModel.DataAnnotations;

namespace RPGOnline.Application.DTOs.Requests.Asset.Spell
{
    public class PostSpellRequest
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
        [MaxLength(40)]
        public string Name { get; set; } = null!;

        [Required]
        [MinLength(1)]
        [MaxLength(280)]
        public string Description { get; set; } = null!;

        [Required]
        [MaxLength(40)]
        public string KeySkill { get; set; } = null!;

        [Required]
        [Range(0, 20)]
        public int MinValue { get; set; }

        [Required]
        [Range(0, 20)]
        public int ManaCost { get; set; }

        [Required]
        [MinLength(1)]
        [MaxLength(280)]
        public string Effects { get; set; } = null!;
    }
}
