using RPGOnline.Application.DTOs.Requests.Forum;
using RPGOnline.Application.DTOs.Responses;

namespace RPGOnline.Application.Interfaces
{
    public interface IPost
    {
        Task<PostDetailsResponse> GetPostDetails(int uId, int postId);
        Task<(ICollection<PostResponse>, int pageCount)> GetPosts(int uId, SearchPostRequest postRequest, CancellationToken cancellationToken);
        Task<Object> PostPost(PostRequest postRequest);
        Task<CommentResponse> PostComment(int id, CommentRequest commentRequest);
        Task<object> DeletePost(int postId, int userId, bool isAdmin);
        Task<object> DeleteComment(int commentId, int userId, bool isAdmin);
    }
}
