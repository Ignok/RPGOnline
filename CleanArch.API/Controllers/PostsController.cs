using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
    }
}
