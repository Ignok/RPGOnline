using System.ComponentModel.DataAnnotations;

namespace RPGOnline.Application.DTOs.Requests.Friendship
{
    public class FriendshipRatingRequest
    {
        [Required]
        public int UId { get; set; }

        [Required]
        public int TargetUId { get; set; }

        [Required]
        
        public byte Rating { get; set; }
    }
}
