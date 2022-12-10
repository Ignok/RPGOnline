using Microsoft.EntityFrameworkCore;
using RPGOnline.Application.Common.Interfaces;
using RPGOnline.Application.DTOs.Requests;
using RPGOnline.Application.DTOs.Requests.Mail;
using RPGOnline.Application.DTOs.Responses;
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

        private readonly int messagesOnPage = 10;

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
                                        SenderUsername = sender.Username,
                                        Title = message.Title,
                                        Content = message.Content,
                                        SendDate = message.SendDate
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
            else
            {
                var message = new Message()
                {
                    MessageId = (_dbContext.Messages.Max(m => (int)m.MessageId) + 1), //potem jak dodamy automatyczny id można usunąć
                    SenderUId = senderId,
                    ReceiverUId = await _dbContext.Users.Where(u => u.Username.Equals(messageRequest.ReceiverUsername)).Select(u => u.UId).FirstOrDefaultAsync(),
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

        public async Task<CommonResponse> DeleteMessage(int uId, int messageId)
        {
            ////////////////////////////////////////
            //autoryzaja i autentykacja do zrobienia

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

            ////////////////////////////

            _dbContext.Messages.Remove(message);
            _dbContext.SaveChanges();


            return new CommonResponse
            {
                Message = "Message deleted successfully"
            };
        }
    }
}
