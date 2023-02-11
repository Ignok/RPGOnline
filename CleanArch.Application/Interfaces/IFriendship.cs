using RPGOnline.Application.DTOs.Requests.Friendship;
using RPGOnline.Application.DTOs.Responses.Friendship;

namespace RPGOnline.Application.Interfaces
{
    public interface IFriendship
    {
        Task<ICollection<UserFriendshipResponse>> GetUserFriends(int uId, int targetId);
        Task<FriendshipResponse> ManageFriendship(FriendshipRequest friendshipRequest);
        Task<FriendshipResponse> GetFriendship(int uId, int targetUId);
        Task<FriendshipResponse> ManageRating(FriendshipRatingRequest friendshipRatingRequest);
    }
}
