using System.ComponentModel.DataAnnotations;

namespace RPGOnline.Application.DTOs.Requests.Asset.Race
{
    public class PostRaceRequest
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
        public string RaceName { get; set; } = null!;

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
    }
}
