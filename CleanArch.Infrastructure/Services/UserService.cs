using Microsoft.EntityFrameworkCore;
using RPGOnline.Application.Common.Interfaces;
using RPGOnline.Application.DTOs.Requests;
using RPGOnline.Application.DTOs.Requests.Asset;
using RPGOnline.Application.DTOs.Requests.User;
using RPGOnline.Application.DTOs.Responses;
using RPGOnline.Application.DTOs.Responses.User;
using RPGOnline.Application.Interfaces;
using RPGOnline.Domain.Enums;

namespace RPGOnline.Infrastructure.Services
{
    public class UserService : IUser
    {
        private readonly IApplicationDbContext _dbContext;
        public UserService(IApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<UserAboutmeResponse> GetAboutMe(int id)
        {
            var result = await _dbContext.Users
                .Where(u => u.UId == id)
                .Select(u => new UserAboutmeResponse()
                {
                    UId = u.UId,
                    Username = u.Username,
                    Picture = u.Picture,
                    Email = u.Email,
                    Country = u.Country,
                    City = u.City,
                    AboutMe = u.AboutMe,
                    Attitude = u.Attitude,
                    CreationDate = u.CreationDate
                }).SingleOrDefaultAsync();

            if (result == null)
            {
                throw new ArgumentNullException($"There is no user with id {id}");
            }

            return result;
        }

        public async Task<PutUserResponse> PutUser(int id, UserRequest userRequest)
        {
            var user = await _dbContext.Users.Where(u => u.UId == id).FirstOrDefaultAsync();
            if (user == null)
            {
                throw new ArgumentNullException($"User {id} does not exist");
            }
            else
            {
                user.Country = userRequest.Country;
                user.City = userRequest.City;
                user.AboutMe = userRequest.AboutMe;
                user.Attitude = userRequest.Attitude;

                _dbContext.Entry(user).State = EntityState.Modified;
                _dbContext.SaveChanges();

                return new PutUserResponse()
                {
                    Country = user.Country,
                    City = user.City,
                    AboutMe = user.AboutMe,
                    Attitude = user.Attitude
                };
            }
        }

        public async Task<ICollection<UserFriendshipResponse>> GetUserFriends(int id)
        {
            var user = await _dbContext.Users.Where(u => u.UId == id).FirstOrDefaultAsync();
            if (user == null)
            {
                throw new ArgumentNullException($"User {id} does not exist");
            }
            else
            {
                var result = await _dbContext.Friendships
                    .Include(f => f.UIdNavigation)
                    .Include(f => f.FriendU)
                    .Where(f => f.UIdNavigation.UId == id)
                    .Select(f => new UserFriendshipResponse()
                    {
                        UId = f.FriendU.UId,
                        Username = f.FriendU.Username,
                        Picture = f.FriendU.Picture,
                        Country = f.FriendU.Country,
                        Attitude = f.FriendU.Attitude,
                        IsFriend = f.IsFriend,
                        IsFollowed = f.IsFollowed,
                        IsBlocked = f.IsBlocked,
                        IsRequestSent = f.IsRequestSent,
                        IsRequestReceived = f.IsRequestReceived,
                    }).ToListAsync();

               /* var friendsIds = await _dbContext.Users
                .Where(u => u.UId == id)
                .SelectMany(u => u.FriendshipUIdNavigations
                                .Where(f => f.UId == id)
                                .Select(f => f.FriendUId)
                            )
                .ToListAsync();*/


                /*var result = await _dbContext.Users
                    .Where(u => friendsIds.Contains(u.UId))
                    .Select(u => new UserFriendshipResponse()
                    {
                        UId = u.UId,
                        Username = u.Username,
                        Picture = u.Picture,
                        Country = u.Country,
                        Attitude = u.Attitude,

                    }).ToListAsync();*/

                return result;
            };
        }

        public async Task<UserResponse> PutAvatar(int id, AvatarRequest avatarRequest)
        {
            var user = await _dbContext.Users.Where(u => u.UId == id).FirstOrDefaultAsync();
            if (user == null)
            {
                throw new ArgumentNullException($"User {id} does not exist");
            }
            else
            {
                user.Picture = avatarRequest.Picture;

                _dbContext.Entry(user).State = EntityState.Modified;
                _dbContext.SaveChanges();

                return new UserResponse()
                {
                    UId = user.UId,
                    Username = user.Username,
                    Picture = user.Picture,
                };
            }
        }

        public async Task<object> ManageFriendship(FriendshipRequest friendshipRequest)
        {
            //if friendship option exists
            if (!Enum.IsDefined(typeof(Friendship), friendshipRequest.Option))
            {
                throw new InvalidDataException($"Friendship option '{friendshipRequest.Option}' is not supported");
            }

            var friendshipStatus = await _dbContext.Friendships.Where(f => f.UId == friendshipRequest.UId && f.FriendUId == friendshipRequest.TargetUId).FirstOrDefaultAsync();

            var viceFriendshipStatus = await _dbContext.Friendships.Where(f => f.UId == friendshipRequest.TargetUId && f.FriendUId == friendshipRequest.UId).FirstOrDefaultAsync();

            if (friendshipStatus == null)
            {
                friendshipStatus = new Domain.Models.Friendship
                {
                    UId = friendshipRequest.UId,
                    FriendUId = friendshipRequest.TargetUId,
                    IsFriend = false,
                    IsFollowed = false,
                    IsBlocked = false,
                    IsRequestSent = false,
                    IsRequestReceived = false,
                };
                _dbContext.Friendships.Add(friendshipStatus);
            }
            if (viceFriendshipStatus == null)
            {
                viceFriendshipStatus = new Domain.Models.Friendship
                {
                    UId = friendshipRequest.TargetUId,
                    FriendUId = friendshipRequest.UId,
                    IsFriend = false,
                    IsFollowed = false,
                    IsBlocked = false,
                    IsRequestSent = false,
                    IsRequestReceived = false,
                };
                _dbContext.Friendships.Add(viceFriendshipStatus);
            }

            //can do?
            switch (Enum.Parse(typeof(Friendship), friendshipRequest.Option))
            {
                case (Friendship.follow):
                    if (viceFriendshipStatus.IsBlocked)
                    {
                        throw new Exception("Target user has blocked you");
                    }
                    else if (friendshipStatus.IsBlocked)
                    {
                        throw new Exception("Target user is blocked");
                    }
                    else if (friendshipStatus.IsFollowed)
                    {
                        throw new Exception("Target user is already followed");
                    }
                    friendshipStatus.IsFollowed = true;
                    break;


                case (Friendship.unfollow):
                    if (viceFriendshipStatus.IsBlocked)
                    {
                        throw new Exception("Target user has blocked you");
                    }
                    else if (friendshipStatus.IsBlocked)
                    {
                        throw new Exception("Target user is blocked");
                    }
                    else if (!friendshipStatus.IsFollowed)
                    {
                        throw new Exception("Target user is not followed");
                    }
                    friendshipStatus.IsFollowed = false;
                    break;


                case (Friendship.friend):
                    if (viceFriendshipStatus.IsBlocked)
                    {
                        throw new Exception("Target user has blocked you");
                    }
                    else if (friendshipStatus.IsBlocked)
                    {
                        throw new Exception("Target user is blocked");
                    }
                    else if (friendshipStatus.IsFriend)
                    {
                        throw new Exception("Target user is already a friend");
                    }
                    else if (friendshipStatus.IsRequestSent)
                    {
                        throw new Exception("Request is already sent");
                    }
                    else if (friendshipStatus.IsRequestReceived)
                    {
                        friendshipStatus.IsFriend = true;
                        viceFriendshipStatus.IsFriend = true;
                        friendshipStatus.IsRequestReceived = false;
                        viceFriendshipStatus.IsRequestSent = false;
                        friendshipStatus.IsFollowed = true;
                        viceFriendshipStatus.IsFollowed = true;

                    }
                    else
                    {
                        friendshipStatus.IsRequestSent = true;
                        viceFriendshipStatus.IsRequestReceived = true;
                    }
                    
                    break;


                case (Friendship.unfriend):
                    if (viceFriendshipStatus.IsBlocked)
                    {
                        throw new Exception("Target user has blocked you");
                    }
                    else if (friendshipStatus.IsBlocked)
                    {
                        throw new Exception("Target user is blocked");
                    }
                    else if (!friendshipStatus.IsFriend)
                    {
                        throw new Exception("Target user is not a friend");
                    }
                    else if (friendshipStatus.IsRequestSent)    //cancelling request
                    {
                        friendshipStatus.IsRequestSent= false;
                        viceFriendshipStatus.IsRequestReceived= false;
                    }
                    else if (viceFriendshipStatus.IsRequestSent) //refusing request
                    {
                        viceFriendshipStatus.IsRequestSent = false;
                        friendshipStatus.IsRequestReceived = false;
                    }
                    else
                    {
                        friendshipStatus.IsFriend = false;
                        viceFriendshipStatus.IsFriend = false;
                    }
                    break;


                case (Friendship.block):
                    if (friendshipStatus.IsBlocked)
                    {
                        throw new Exception("Target user is already blocked");
                    }
                    friendshipStatus.IsBlocked = true;
                    if (!viceFriendshipStatus.IsBlocked)
                    {
                        viceFriendshipStatus.IsFriend = false;
                        viceFriendshipStatus.IsRequestReceived = false;
                        viceFriendshipStatus.IsRequestSent = false;
                        viceFriendshipStatus.IsFollowed = false;

                    }
                    friendshipStatus.IsFollowed = false;
                    friendshipStatus.IsRequestSent = false;
                    friendshipStatus.IsRequestReceived = false;
                    friendshipStatus.IsFriend = false;

                    break;


                case (Friendship.unblock):
                    if (!friendshipStatus.IsBlocked)
                    {
                        throw new Exception("Target user is not blocked.");
                    }
                    friendshipStatus.IsBlocked = false;
                    break;
            }

            if(!friendshipStatus.IsBlocked
                && !friendshipStatus.IsFriend
                && !friendshipStatus.IsFollowed
                && !friendshipStatus.IsRequestSent
                && !friendshipStatus.IsRequestReceived
                && !viceFriendshipStatus.IsBlocked
                && !viceFriendshipStatus.IsFriend
                && !viceFriendshipStatus.IsFollowed
                && !viceFriendshipStatus.IsRequestSent
                && !viceFriendshipStatus.IsRequestReceived)
            {
                _dbContext.Friendships.Remove(friendshipStatus);
                _dbContext.Friendships.Remove(viceFriendshipStatus);
            }



            _dbContext.SaveChanges();

            return new
            {
                message = "Successfully changed status"
            };
        }


        //Assets save and unsave
        public async Task<object> PostSaveAsset(int uId, int assetId)
        {
            var asset = await _dbContext.Assets.Where(a => a.AssetId == assetId).Where(a => a.IsPublic || a.AuthorId == uId).FirstOrDefaultAsync();
            if (asset == null)
            {
                throw new Exception("Asset does not exist or it is private");
            }

            var usa = await _dbContext.UserSavedAssets.Where(usa => usa.UId == uId && usa.AssetId == assetId).FirstOrDefaultAsync();
            if (usa != null)
            {
                throw new Exception("User's already saved this asset");
            }

            

            var userSavedAsset = new Domain.Models.UserSavedAsset()
            {
                UId = uId,
                AssetId = assetId,
                SaveDate = DateTime.Now,
            };

            _dbContext.UserSavedAssets.Add(userSavedAsset);
            _dbContext.SaveChanges();

            return "Asset saved";
        }

        public async Task<object> DeleteSaveAsset(int uId, int assetId)
        {
            var asset = await _dbContext.Assets.Where(a => a.AssetId == assetId).Where(a => a.IsPublic || a.AuthorId == uId).FirstOrDefaultAsync();
            if (asset == null)
            {
                throw new Exception("Asset does not exist or it is private");
            }

            var usa = await _dbContext.UserSavedAssets.Where(usa => usa.UId == uId && usa.AssetId == assetId).FirstOrDefaultAsync();
            if (usa == null)
            {
                throw new Exception("User's not saved this asset yet");
            }

            _dbContext.UserSavedAssets.Remove(usa);
            _dbContext.SaveChanges();

            return "Asset unsaved";
        }
    }
}
