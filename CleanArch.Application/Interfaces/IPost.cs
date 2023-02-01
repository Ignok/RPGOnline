using RPGOnline.Application.DTOs.Requests;
using RPGOnline.Application.DTOs.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGOnline.Application.Interfaces
{
    public interface IPost
    {
        Task<PostDetailsResponse> GetPostDetails(int uId, int postId);
        Task<(ICollection<PostResponse>, int pageCount)> GetPosts(int uId, SearchPostRequest postRequest, CancellationToken cancellationToken);
        Task<Object> PostPost(PostRequest postRequest);
        Task<CommentResponse> PostComment(int id, CommentRequest commentRequest);
    }
}
