using System.ComponentModel.DataAnnotations;

namespace RPGOnline.Application.DTOs.Requests.Asset.Item
{
    public class PostItemRequest
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
        public string ItemMane { get; set; } = null!;

        [Required]
        [MinLength(1)]
        [MaxLength(280)]
        public string Description { get; set; } = null!;

        [Required]
        [MaxLength(40)]
        public string KeySkill { get; set; } = null!;

        [Required]
        [Range(0,6)]
        public int SkillMod { get; set; }

        [Required]
        [Range(0, 100)]
        public int GoldMultiplier { get; set; }
    }
}
