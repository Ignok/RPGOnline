using System.ComponentModel.DataAnnotations;

namespace RPGOnline.Application.DTOs.Requests.Asset.Character.FromJson
{
    public class CharacteristicsRequest
    {
        [Required]
        [MinLength(1)]
        [MaxLength(40)]
        public string Voice { get; set; } = null!;

        [Required]
        [MinLength(1)]
        [MaxLength(40)]
        public string Posture { get; set; } = null!;

        [Required]
        [MinLength(1)]
        [MaxLength(40)]
        public string Temperament { get; set; } = null!;

        [Required]
        [MinLength(1)]
        [MaxLength(40)]
        public string Beliefs { get; set; } = null!;

        [Required]
        [MinLength(1)]
        [MaxLength(40)]
        public string Face { get; set; } = null!;

        [Required]
        [MinLength(1)]
        [MaxLength(40)]
        public string Origins { get; set; } = null!;
    }
}
