using RPGOnline.Application.DTOs.Requests;
using RPGOnline.Application.DTOs.Responses;

namespace RPGOnline.Application.Interfaces
{
    public interface IMessage
    {
        Task<ICollection<MessageResponse>> GetUserMessages(int id);

        Task<MessageResponse> PostMessage(int senderId, MessageRequest messageRequest);

        Task<CommonResponse> DeleteMessage(int uId, int messageId);
    }
}
