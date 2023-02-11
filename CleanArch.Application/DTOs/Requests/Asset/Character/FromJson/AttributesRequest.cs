using System.ComponentModel.DataAnnotations;

namespace RPGOnline.Application.DTOs.Requests.Asset.Character.FromJson
{
    public class AttributesRequest
    {
        [Required]
        [Range(0, 20)]
        public int Strength { get; set; }

        [Required]
        [Range(0, 20)]
        public int Dexterity { get; set; }

        [Required]
        [Range(0, 20)]
        public int Intelligence { get; set; }

        [Required]
        [Range(0, 20)]
        public int Charisma { get; set; }

        [Required]
        [Range(0, 20)]
        public int Health { get; set; }

        [Required]
        [Range(0, 20)]
        public int Mana { get; set; }
    }
}
