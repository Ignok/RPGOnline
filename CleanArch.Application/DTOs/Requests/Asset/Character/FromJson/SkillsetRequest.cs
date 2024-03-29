﻿using System.ComponentModel.DataAnnotations;

namespace RPGOnline.Application.DTOs.Requests.Asset.Character.FromJson
{
    public class SkillsetRequest
    {
        [Required]
        [Range(-6, 6)]
        public int Weapon { get; set; }

        [Required]
        [Range(-6, 6)]
        public int Armor { get; set; }

        [Required]
        [Range(-6, 6)]
        public int Gadget { get; set; }

        [Required]
        [Range(-6, 6)]
        public int Companion { get; set; }

        [Required]
        [Range(-6, 6)]
        public int Psyche { get; set; }
    }
}
