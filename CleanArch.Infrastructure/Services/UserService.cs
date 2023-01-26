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

        public async Task<ICollection<UserAboutmeResponse>> GetUserFriends(int id)
        {
            var user = await _dbContext.Users.Where(u => u.UId == id).FirstOrDefaultAsync();
            if (user == null)
            {
                throw new ArgumentNullException($"User {id} does not exist");
            }
            else
            {
                var friendsIds = await _dbContext.Users
                .Where(u => u.UId == id)
                .SelectMany(u => u.FriendshipUIdNavigations
                                .Where(f => f.UId == id)
                                .Where(f => f.FriendshipStatus == 1 || f.FriendshipStatus == 2)
                                .Select(f => f.FriendUId)
                            )
                .ToListAsync();


                var result = await _dbContext.Users
                    .Where(u => friendsIds.Contains(u.UId))
                    .Select(u => new UserAboutmeResponse()
                    {
                        UId = u.UId,
                        Username = u.Username,
                        Picture = u.Picture,
                        Country = u.Country,
                        Attitude = u.Attitude,
                    }).ToListAsync();

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
                    FriendshipStatus = 0,
                    IsFollowed = 0
                };
                _dbContext.Friendships.Add(friendshipStatus);
            }
            if (viceFriendshipStatus == null)
            {
                viceFriendshipStatus = new Domain.Models.Friendship
                {
                    UId = friendshipRequest.TargetUId,
                    FriendUId = friendshipRequest.UId,
                    FriendshipStatus = 0,
                    IsFollowed = 0
                };
                _dbContext.Friendships.Add(viceFriendshipStatus);
            }

            //can do?
            switch (Enum.Parse(typeof(Friendship), friendshipRequest.Option))
            {
                case (Friendship.follow):
                    if (viceFriendshipStatus.FriendshipStatus == -1)
                    {
                        throw new Exception("Target user has blocked you");
                    }
                    else if (friendshipStatus.FriendshipStatus == -1)
                    {
                        throw new Exception("Target user is blocked");
                    }
                    else if (friendshipStatus.IsFollowed == 1)
                    {
                        throw new Exception("Target user is already followed");
                    }
                    friendshipStatus.IsFollowed = 1;
                    break;


                case (Friendship.unfollow):
                    if (viceFriendshipStatus.FriendshipStatus == -1)
                    {
                        throw new Exception("Target user has blocked you");
                    }
                    else if (friendshipStatus.FriendshipStatus == -1)
                    {
                        throw new Exception("Target user is blocked");
                    }
                    else if (friendshipStatus.IsFollowed == 0)
                    {
                        throw new Exception("Target user is not followed");
                    }
                    friendshipStatus.IsFollowed = 0;
                    break;


                case (Friendship.friend):
                    if (viceFriendshipStatus.FriendshipStatus == -1)
                    {
                        throw new Exception("Target user has blocked you");
                    }
                    else if (friendshipStatus.FriendshipStatus == -1)
                    {
                        throw new Exception("Target user is blocked");
                    }
                    else if (friendshipStatus.FriendshipStatus == 1)
                    {
                        throw new Exception("Target user is already a friend");
                    }
                    friendshipStatus.FriendshipStatus = 1;
                    break;


                case (Friendship.unfriend):
                    if (viceFriendshipStatus.FriendshipStatus == -1)
                    {
                        throw new Exception("Target user has blocked you");
                    }
                    else if (friendshipStatus.FriendshipStatus == -1)
                    {
                        throw new Exception("Target user is blocked");
                    }
                    else if (friendshipStatus.FriendshipStatus == 0)
                    {
                        throw new Exception("Target user is not a friend");
                    }
                    friendshipStatus.FriendshipStatus = 0;
                    break;


                case (Friendship.block):
                    if (friendshipStatus.FriendshipStatus == -1)
                    {
                        throw new Exception("Target user is already blocked");
                    }
                    friendshipStatus.FriendshipStatus = -1;
                    if (viceFriendshipStatus.FriendshipStatus != -1)
                    {
                        viceFriendshipStatus.FriendshipStatus = 0;
                    }
                    friendshipStatus.IsFollowed = 0;
                    viceFriendshipStatus.IsFollowed = 0;
                    break;


                case (Friendship.unblock):
                    if (friendshipStatus.FriendshipStatus != -1)
                    {
                        throw new Exception("Target user is not blocked.");
                    }
                    friendshipStatus.FriendshipStatus = 0;
                    break;
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
