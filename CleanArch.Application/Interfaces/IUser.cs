using RPGOnline.Application.DTOs.Requests;
using RPGOnline.Application.DTOs.Requests.Asset;
using RPGOnline.Application.DTOs.Requests.User;
using RPGOnline.Application.DTOs.Responses;
using RPGOnline.Application.DTOs.Responses.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGOnline.Application.Interfaces
{
    public interface IUser
    {
        Task<UserAboutmeResponse> GetAboutMe(int id);

        Task<PutUserResponse> PutUser(int id, UserRequest userRequest);

        Task<UserSimplifiedResponse> PutAvatar(int id, AvatarRequest avatarRequest);

        Task<ICollection<UserFriendshipResponse>> GetUserFriends(int id);
        Task<object> ManageFriendship(FriendshipRequest friendshipRequest);

        Task<object> PostSaveAsset(int uId, int assetId);
        Task<object> DeleteSaveAsset(int uId, int assetId);
    }
}
