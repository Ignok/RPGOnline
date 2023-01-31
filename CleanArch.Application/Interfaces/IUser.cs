using RPGOnline.Application.DTOs.Requests;
using RPGOnline.Application.DTOs.Requests.User;
using RPGOnline.Application.DTOs.Responses;
using RPGOnline.Application.DTOs.Responses.User;

namespace RPGOnline.Application.Interfaces
{
    public interface IUser
    {
        Task<ICollection<UserResponse>> GetUsers(SearchUserRequest userRequest, int userId, CancellationToken cancellationToken);
        Task<UserAboutmeResponse> GetAboutMe(int uId, int targetId);

        Task<PutUserResponse> PutUser(int id, UserRequest userRequest);

        Task<UserSimplifiedResponse> PutAvatar(int id, AvatarRequest avatarRequest);

        Task<object> PostSaveAsset(int uId, int assetId);
        Task<object> DeleteSaveAsset(int uId, int assetId);
    }
}
