﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.TeamFoundation;
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
            try
            {
                var result = await _postService.GetPosts(postRequest);

                if (result == (null, null))
                {
                    return NotFound("No posts in database.");
                }
                else
                {
                    return Ok(new
                    {
                        result.Item1,
                        result.pageCount
                    });
                }
            }catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPost(int id)
        {
            try
            {
                var result = await _postService.GetPostDetails(id);

                if (result == null)
                {
                    return NotFound("No such post in database.");
                }
                return Ok(result);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> PostPost(PostRequest postRequest)
        {
            try
            {
                var result = await _postService.PostPost(postRequest);

                if (result == null)
                {
                    return BadRequest(result);
                }
                return Ok("Post has been uploaded");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
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
