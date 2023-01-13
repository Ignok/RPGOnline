using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGOnline.Application.DTOs.Requests.User
{
    public class FriendshipRequest
    {
        [Required]
        public int UId { get; set; }

        [Required]
        public int TargetUId { get; set; }

        [Required]
        public string Option { get; set; } = null!;
    }
}
