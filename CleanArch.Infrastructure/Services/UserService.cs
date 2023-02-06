using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RPGOnline.Application.Common.Interfaces;
using RPGOnline.Application.DTOs.Requests;
using RPGOnline.Application.DTOs.Requests.User;
using RPGOnline.Application.DTOs.Responses;
using RPGOnline.Application.DTOs.Responses.Friendship;
using RPGOnline.Application.DTOs.Responses.User;
using RPGOnline.Application.Interfaces;
using RPGOnline.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace RPGOnline.Infrastructure.Services
{
    public class UserService : IUser
    {
        private readonly IApplicationDbContext _dbContext;
        private ILogger<UserService> _logger;
        public UserService(IApplicationDbContext dbContext, ILogger<UserService> logger)
        {
            _logger = logger;
            _dbContext = dbContext;
        }


        public async Task<ICollection<UserResponse>> GetUsers(SearchUserRequest userRequest, int userId, CancellationToken cancellationToken)
        {
            try
            {

                var result = _dbContext.Users
                    .Include(u => u.FriendshipUIdNavigations)
                    .Include(u => u.FriendshipFriendUs)
                    .AsParallel().WithCancellation(cancellationToken)
                    //.Where(u => attitude.Equals(null) || u.Attitude.Equals(attitude))
                    .Where(u => String.IsNullOrEmpty(userRequest.Attitude)
                                || object.Equals(u.Attitude, userRequest.Attitude))
                    .Where(u => String.IsNullOrEmpty(userRequest.Search)
                                || (u.Username.Contains(userRequest.Search, StringComparison.OrdinalIgnoreCase))
                                || ((u.AboutMe ?? "").Contains(userRequest.Search, StringComparison.OrdinalIgnoreCase))
                            )
                    .OrderBy(u => u.CreationDate)
                    //// do dodania rating
                    .Select(u => new UserResponse()
                    {
                        UId = u.UId,
                        Username = u.Username,
                        Picture = u.Picture,
                        AboutMe = u.AboutMe,
                        Attitude = u.Attitude,
                        HasBlockedMe = u.FriendshipUIdNavigations
                                        .Where(u => u.FriendUId == userId)
                                        .Where(u => u.IsBlocked).Any(),
                        AverageRating = u.FriendshipFriendUs.Where(f => f.Rating != 0).Select(f => (int)f.Rating).DefaultIfEmpty().Average(),
                    }).ToList();


                await Task.Delay(500, cancellationToken);

                return result;
            }
            catch (TaskCanceledException ex)
            {
                _logger.LogError("=========== I WAS CANCELLED ==========");
                throw ex;
            }
            

            throw new NotImplementedException();

        }

        public async Task<UserAboutmeResponse> GetAboutMe(int uId, int targetId)
        {
            if(HasBlockedMe(uId, targetId))
            {
                throw new ArgumentException("Blocked");
            }

            var result = await _dbContext.Users
                .Include(u => u.FriendshipUIdNavigations)
                .Include(u => u.FriendshipFriendUs)
                .Where(u => u.UId == targetId)
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
                    CreationDate = u.CreationDate,
                    FriendshipStatus = new FriendshipResponse()
                    {
                        IsFriend = u.FriendshipFriendUs.Where(u => u.UId == uId).Where(u => u.IsFriend).Any(),
                        IsBlocked = u.FriendshipFriendUs.Where(u => u.UId == uId).Where(u => u.IsBlocked).Any(),
                        IsFollowed = u.FriendshipFriendUs.Where(u => u.UId == uId).Where(u => u.IsFollowed).Any(),
                        IsRequestReceived = u.FriendshipFriendUs.Where(u => u.UId == uId).Where(u => u.IsRequestReceived).Any(),
                        IsRequestSent = u.FriendshipFriendUs.Where(u => u.UId == uId).Where(u => u.IsRequestSent).Any(),
                        HasBlockedMe = u.FriendshipUIdNavigations
                                        .Where(u => u.FriendUId == uId)
                                        .Where(u => u.IsBlocked).Any(),
                        MyRating = u.FriendshipFriendUs.Where(u => u.UId == uId).Select(u => u.Rating).FirstOrDefault(),
                        AverageRating = u.FriendshipFriendUs.Where(f => f.Rating != 0).Select(f => (int)f.Rating).DefaultIfEmpty().Average(),
                    }
                        
                }).SingleOrDefaultAsync();

            if (result == null)
            {
                throw new ArgumentNullException($"There is no user with id {targetId}");
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
        

        public async Task<UserSimplifiedResponse> PutAvatar(int id, AvatarRequest avatarRequest)
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

                return new UserSimplifiedResponse()
                {
                    UId = user.UId,
                    Username = user.Username,
                    Picture = user.Picture,
                };
            }
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



        //Assets save and unsave
        public async Task<object> PostSavePost(int uId, int postId)
        {
            var post = await _dbContext.Posts.Where(p => p.PostId == postId).FirstOrDefaultAsync();
            if (post == null)
            {
                throw new Exception("Post does not exist");
            }

            var ulp = await _dbContext.UserLikedPosts.Where(ulp => ulp.UId == uId && ulp.PostId == postId).FirstOrDefaultAsync();
            if (ulp != null)
            {
                throw new Exception("User's already liked this post");
            }



            var userLikedPost = new Domain.Models.UserLikedPost()
            {
                UId = uId,
                PostId = postId,
                LikeDate = DateTime.Now,
            };

            _dbContext.UserLikedPosts.Add(userLikedPost);
            _dbContext.SaveChanges();

            return "Post liked";
        }

        public async Task<object> DeleteSavePost(int uId, int postId)
        {
            var post = await _dbContext.Posts.Where(p => p.PostId == postId).FirstOrDefaultAsync();
            if (post == null)
            {
                throw new Exception("Post does not exist");
            }

            var ulp = await _dbContext.UserLikedPosts.Where(ulp => ulp.UId == uId && ulp.PostId == postId).FirstOrDefaultAsync();
            if (ulp == null)
            {
                throw new Exception("User's already liked this post");
            }

            _dbContext.UserLikedPosts.Remove(ulp);
            _dbContext.SaveChanges();

            return "Post unliked";
        }



        private bool HasBlockedMe(int myId, int targetId)
        {
            if(myId == targetId) return false;
            return myId == targetId || _dbContext.Friendships
                .Where(f => f.UId == targetId && f.FriendUId == myId)
                .Where(f => f.IsBlocked).Any();
        }
    }
}
