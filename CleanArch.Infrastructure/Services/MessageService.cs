using Microsoft.EntityFrameworkCore;
using RPGOnline.Application.Common.Interfaces;
using RPGOnline.Application.DTOs.Requests;
using RPGOnline.Application.DTOs.Requests.Mail;
using RPGOnline.Application.DTOs.Responses;
using RPGOnline.Application.DTOs.Responses.Mail;
using RPGOnline.Application.Interfaces;
using RPGOnline.Domain.Models;


namespace RPGOnline.Infrastructure.Services
{
    public class MessageService : IMessage
    {
        private readonly IApplicationDbContext _dbContext;
        public MessageService(IApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        private readonly int messagesOnPage = 5;

        public async Task<(ICollection<MessageResponse>, int pageCount)> GetUserMessages(int id, GetMessageRequest getMessageRequest)
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
                                    orderby message.SendDate descending
                                    select new MessageResponse()
                                    {
                                        MessageId = message.MessageId,
                                        SenderId = message.SenderUId,
                                        SenderPicture = sender.Picture,
                                        SenderUsername = sender.Username,
                                        Title = message.Title,
                                        Content = message.Content,
                                        SendDate = message.SendDate,
                                        IsOpened = message.IsOpened,
                                    })
                            .ToListAsync();

                int pageCount = (int)Math.Ceiling((double)result.Count / messagesOnPage);

                result = result
                    .Skip(messagesOnPage * (getMessageRequest.Page - 1))
                    .Take(messagesOnPage)
                    .ToList();

                return (result, pageCount);
            };
        }

        public async Task<MessageResponse> PostMessage(int senderId, PostMessageRequest messageRequest)
        {

            if (messageRequest == null)
                throw new ArgumentNullException("Message cannot be null");



            if (await _dbContext.Users.Where(u => u.UId == senderId).FirstOrDefaultAsync() == null)
            {
                throw new ArgumentNullException($"User id {senderId} does not exist");
            }
            var receiver = await _dbContext.Users.Where(u => u.Username.Equals(messageRequest.ReceiverUsername)).Select(u => u).FirstOrDefaultAsync();

            if (receiver == null)
            {
                Exception e = new Exception();
                e.Data.Add("Username", $"Username {messageRequest.ReceiverUsername} does not exist");
                throw e;
            }
            else if (HasBlockedMe(senderId, receiver.UId))
            {
                Exception e = new Exception();
                e.Data.Add("Username", "Error - this user has blocked you");
                throw e;
            }
            else
            {
                var message = new Message()
                {
                    MessageId = (_dbContext.Messages.Max(m => (int)m.MessageId) + 1),
                    SenderUId = senderId,
                    ReceiverUId = receiver.UId,
                    Title = messageRequest.Title,
                    Content = messageRequest.Content,
                    SendDate = DateTime.Now,
                    IsOpened = false,
                };

                _dbContext.Messages.Add(message);
                _dbContext.SaveChanges();

                return new MessageResponse()
                {
                    MessageId = message.MessageId,
                    SenderId = message.SenderUId,
                    Title = message.Title,
                    Content = message.Content,
                    SendDate = message.SendDate,
                    IsOpened = false,
                };
            }
        }

        public async Task<CommonResponse> DeleteMessage(int uId, int messageId)
        {

            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.UId == uId);
            if(user == null)
            {
                throw new Exception($"User id {uId} does not exist");
            }

            var message = await _dbContext.Messages.FirstOrDefaultAsync(m => m.MessageId == messageId);
            if(message == null)
            {
                throw new Exception($"Message id {messageId} does not exist");
            }

            if(message.ReceiverUId != uId)
            {
                throw new Exception("Permisiion denied - user is not receiver");
            }

            _dbContext.Messages.Remove(message);
            _dbContext.SaveChanges();


            return new CommonResponse
            {
                Message = "Message successfully deleted"
            };
        }

        public async Task<CommonResponse> OpenMessage(int uId, int messageId)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.UId == uId);
            if (user == null)
            {
                throw new Exception($"User id {uId} does not exist");
            }

            var message = await _dbContext.Messages.FirstOrDefaultAsync(m => m.MessageId == messageId);
            if (message == null)
            {
                throw new Exception($"Message id {messageId} does not exist");
            }

            if (message.ReceiverUId != uId)
            {
                throw new Exception("Permisiion denied - user is not receiver");
            }

            if (message.IsOpened)
            {
                throw new Exception("Message is already opened");
            }

            message.IsOpened = true;

            _dbContext.Entry(message).State = EntityState.Modified;
            _dbContext.SaveChanges();

            return new CommonResponse
            {
                Message = "Message successfully opened"
            };
        }

        public async Task<CommonResponse> CloseMessage(int uId, int messageId)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.UId == uId);
            if (user == null)
            {
                throw new Exception($"User id {uId} does not exist");
            }

            var message = await _dbContext.Messages.FirstOrDefaultAsync(m => m.MessageId == messageId);
            if (message == null)
            {
                throw new Exception($"Message id {messageId} does not exist");
            }

            if (message.ReceiverUId != uId)
            {
                throw new Exception("Permisiion denied - user is not receiver");
            }

            if (!message.IsOpened)
            {
                throw new Exception("Message is already closed");
            }

            message.IsOpened = false;

            _dbContext.Entry(message).State = EntityState.Modified;
            _dbContext.SaveChanges();

            return new CommonResponse
            {
                Message = "Message successfully closed"
            };
        }

        public async Task<NewMessagesCountResponse> GetNewMessagesCount(int uId)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.UId == uId);
            if (user == null)
            {
                throw new Exception($"User id {uId} does not exist");
            }

            var count = await _dbContext.Messages
                                .Where(m => m.ReceiverUId == uId)
                                .Where(m => !m.IsOpened)
                                .CountAsync();

            return new NewMessagesCountResponse()
            {
                NewMessagesCount = count
            };


            throw new NotImplementedException();
        }


        private bool HasBlockedMe(int myId, int targetId)
        {
            if (myId == targetId) return false;
            return myId == targetId || _dbContext.Friendships
                .Where(f => f.UId == targetId && f.FriendUId == myId)
                .Where(f => f.IsBlocked).Any();
        }
    }
}
