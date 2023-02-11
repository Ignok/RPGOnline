using System.ComponentModel.DataAnnotations;

namespace RPGOnline.Domain.Enums
{
    public enum Attitude
    {
        [Display(Name ="Epic GM")]
        EpicGM,
        [Display(Name = "New User")]
        NewUser,
        [Display(Name = "Experienced")]
        Experienced,
        [Display(Name = "Adventurous")]
        Adventurous,
        [Display(Name = "Casual Player")]
        CasualPlayer
    }
}
