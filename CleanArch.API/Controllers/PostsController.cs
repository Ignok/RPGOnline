using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RPGOnline.Application.DTOs.Requests;
using RPGOnline.Application.DTOs.Responses;
using RPGOnline.Application.Interfaces;
using RPGOnline.Infrastructure.Models;

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
        /*
        [HttpGet]
        public async Task<IActionResult> GetPosts()
        {
            var result = await _postService.GetPosts();

            if (result == null)
            {
                return BadRequest("No posts in database.");
            }
            else
            {
                return Ok(result);
            }
        }
        */

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

        [HttpPost("Comment")]
        public async Task<IActionResult> PostComment(CommentRequest commentRequest)
        {
            try
            {
                var result = await _postService.PostComment(commentRequest);
                return Ok(result);

            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            

            
           
        }
    }
}
