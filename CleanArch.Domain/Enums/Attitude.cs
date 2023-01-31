using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
