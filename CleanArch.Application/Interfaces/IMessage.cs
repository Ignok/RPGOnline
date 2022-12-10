using RPGOnline.Application.DTOs.Requests;
using RPGOnline.Application.DTOs.Requests.Mail;
using RPGOnline.Application.DTOs.Responses;

namespace RPGOnline.Application.Interfaces
{
    public interface IMessage
    {
        Task<(ICollection<MessageResponse>, int pageCount)> GetUserMessages(int id, GetMessageRequest getMessageRequest);

        Task<MessageResponse> PostMessage(int senderId, PostMessageRequest messageRequest);

        Task<CommonResponse> DeleteMessage(int uId, int messageId);
    }
}
