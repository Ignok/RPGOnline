using System.ComponentModel.DataAnnotations;

namespace RPGOnline.Application.DTOs.Requests.Friendship
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
