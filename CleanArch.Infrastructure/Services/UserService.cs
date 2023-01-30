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
    }
}
