using RPGOnline.Application.DTOs.Requests.Asset.Character.FromJson;
using System.ComponentModel.DataAnnotations;

namespace RPGOnline.Application.DTOs.Requests.Asset.Character
{
    public class PostCharacterRequest
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
        [Range(0, 9999999)]
        public int Gold { get; set; }

        //public string? Avatar { get; set; }

        public virtual FromJsonRequest JsonRequest { get; set; } = null!;

        public int? Race { get; set; }
        public int? Profession { get; set; }

        [Required]
        [MaxLength(40)]
        public string Type { get; set; } = null!;
    }
}
