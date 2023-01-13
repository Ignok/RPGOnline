using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.TeamFoundation;
using RPGOnline.Application.DTOs.Requests;
using RPGOnline.Application.Interfaces;
using System.Security.Claims;

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
        public async Task<IActionResult> GetPosts([FromQuery] SearchPostRequest postRequest, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _postService.GetPosts(postRequest, cancellationToken);

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

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> PostPost(PostRequest postRequest)
        {
            try
            {
                if (!IsSameId(postRequest.UId))
                {
                    return BadRequest("Access denied - bad ID");
                }

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

        [Authorize]
        [HttpPost("{postId}/Comment")]
        public async Task<IActionResult> PostComment(int postId, CommentRequest commentRequest)
        {
            try
            {
                if (!IsSameId(commentRequest.UId))
                {
                    return BadRequest("Access denied - bad ID");
                }

                var result = await _postService.PostComment(postId, commentRequest);
                return Ok(result);

            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private bool IsSameId(int id)
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return userId.Equals(id.ToString());
        }
    }
}
