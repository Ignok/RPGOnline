using RPGOnline.Application.DTOs.Responses.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGOnline.Application.DTOs.Responses.Asset.Race
{
    public class GetRaceResponse
    {
        public int AssetId { get; set; }
        public DateTime CreationDate { get; set; }
        public int TimesSaved { get; set; }
        public int RaceId { get; set; }
        public string RaceName { get; set; } = null!;
        public string RaceDescription { get; set; } = null!;
        public string RaceTalent { get; set; } = null!;
        public string RaceHiddenTalent { get; set; } = null!;
        public string? RaceKeyAttribute { get; set; }
        public string PrefferedLanguage { get; set; } = null!;
        public virtual UserResponse CreatorNavigation { get; set; } = null!;
    }
}
