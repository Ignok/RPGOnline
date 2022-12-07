using Microsoft.EntityFrameworkCore;
using RPGOnline.Application.Common.Interfaces;
using RPGOnline.Application.DTOs.Requests;
using RPGOnline.Application.DTOs.Responses;
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

        public async Task<ICollection<UserResponse>> GetUserFriends(int id)
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
                    .Select(u => new UserResponse()
                    {
                        UId = u.UId,
                        Username = u.Username,
                        Picture = u.Picture
                    }).ToListAsync();
               
                return result;
            };
        }

        public async Task<ICollection<MessageResponse>> GetUserMessages(int id)
        {
            var isUser = await _dbContext.Users.Where(u => u.UId == id).FirstOrDefaultAsync();
            if (isUser == null)
            {
                throw new ArgumentNullException($"User {id} does not exist");
            }
            else
            {
                var result = await (from user in _dbContext.Users
                            join message in _dbContext.Messages on user.UId equals message.ReceiverUId
                            join sender in _dbContext.Users on message.SenderUId equals sender.UId 
                            where user.UId == id
                            select new MessageResponse()
                            {
                                MessageId = message.MessageId,
                                SenderId = message.SenderUId,
                                SenderUsername = sender.Username,
                                Title = message.Title,
                                Content = message.Content,
                                SendDate = message.SendDate
                            }).ToListAsync();

                return result;
            };
        }

        public async Task<MessageResponse> PostMessage(int senderId, MessageRequest messageRequest)
        {

            if (messageRequest == null)
                throw new ArgumentNullException("Message cannot be null");

            

            if (await _dbContext.Users.Where(u => u.UId == senderId).FirstOrDefaultAsync() == null)
            {
                throw new ArgumentNullException($"User id {senderId} does not exist");
            }
            var receiver = await _dbContext.Users.Where(u => u.Username.Equals(messageRequest.ReceiverUsername)).Select(u => u).FirstOrDefaultAsync();

            if(receiver == null)
            {
                Exception e = new Exception();
                e.Data.Add("Username", $"Username {messageRequest.ReceiverUsername} does not exist");
                throw e;
            }
            else
            {
                var message = new Message()
                {
                    MessageId = (_dbContext.Messages.Max(m => (int)m.MessageId) + 1), //potem jak dodamy automatyczny id można usunąć
                    SenderUId = senderId,
                    ReceiverUId = await _dbContext.Users.Where(u => u.UId == senderId || u.Username.Equals(messageRequest.ReceiverUsername)).Select(u => u.UId).FirstOrDefaultAsync(),
                    Title = messageRequest.Title,
                    Content = messageRequest.Content,
                    SendDate = DateTime.Now
                };

                _dbContext.Messages.Add(message);
                _dbContext.SaveChanges();

                return new MessageResponse()
                {
                    MessageId = message.MessageId,
                    SenderId = message.SenderUId,
                    Title = message.Title,
                    Content = message.Content,
                    SendDate = message.SendDate
                };
            }
        }
    }
}
