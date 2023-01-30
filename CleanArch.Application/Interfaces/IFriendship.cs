using RPGOnline.Application.DTOs.Requests.Friendship;
using RPGOnline.Application.DTOs.Responses.Friendship;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGOnline.Application.Interfaces
{
    public interface IFriendship
    {
        Task<ICollection<UserFriendshipResponse>> GetUserFriends(int id);
        Task<object> ManageFriendship(FriendshipRequest friendshipRequest);
        Task<FriendshipResponse> GetFriendship(int uId, int targetUId);
    }
}
