using Microsoft.AspNetCore.Mvc;
using RPGOnline.Application.DTOs.Requests;
using RPGOnline.Application.Interfaces;

namespace RPGOnline.API.Controllers
{
    public class PostsController : CommonController
    {

        private readonly IPost _postService;

        public PostsController(IPost postService)
        {
            _postService = postService;
        }

        [HttpGet]
        public async Task<IActionResult> GetPosts([FromQuery] SearchPostRequest postRequest)
        {
            var result = await _postService.GetPosts(postRequest);

            if (result == (null, null))
            {
                return BadRequest("No posts in database.");
            }
            else
            {
                return Ok(new 
                {
                    result.Item1,
                    result.Item2
                });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPost(int id)
        {
            var result = await _postService.GetPostDetails(id);

            if (result == null)
            {
                return BadRequest("No such post in database.");
            }
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> PostPost(PostRequest postRequest)
        {
            var result = await _postService.PostPost(postRequest);

            if(result == null)
            {
                return BadRequest(result);
            }
            return Ok("Post has been uploaded");
        }

        [HttpPost("{postId}/Comment")]
        public async Task<IActionResult> PostComment(int postId, CommentRequest commentRequest)
        {
            try
            {
                var result = await _postService.PostComment(postId, commentRequest);
                return Ok(result);

            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            

            
           
        }
    }
}
