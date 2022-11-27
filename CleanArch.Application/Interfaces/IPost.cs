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
        Task<PostDetailsResponse> GetPostDetails(int id);
        Task<(ICollection<PostResponse>, int pageCount)> GetPosts(SearchPostRequest postRequest);
        Task<Object> PostPost(PostRequest postRequest);
    }
}
