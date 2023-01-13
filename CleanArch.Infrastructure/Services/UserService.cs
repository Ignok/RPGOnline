using Microsoft.EntityFrameworkCore;
using RPGOnline.Application.Common.Interfaces;
using RPGOnline.Application.DTOs.Requests;
using RPGOnline.Application.DTOs.Requests.User;
using RPGOnline.Application.DTOs.Responses;
using RPGOnline.Application.DTOs.Responses.User;
using RPGOnline.Application.Interfaces;
using RPGOnline.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            if(result == null)
            {
                throw new ArgumentNullException($"There is no user with id {id}");
            }

            return result;
        }

        public async Task<PutUserResponse> PutUser(int id, UserRequest userRequest)
        {
            var user = await _dbContext.Users.Where(u => u.UId == id).FirstOrDefaultAsync();
            if(user == null)
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

        public Task<object> ManageFriendship(FriendshipRequest friendshipRequest)
        {
            //if friendship option exists
            if (!Enum.IsDefined(typeof(Friendship), friendshipRequest.Option))
                throw new InvalidDataException($"Friendship option '{friendshipRequest.Option}' is not supported");

            //if user is blocked



            throw new NotImplementedException();
        }
    }
}
