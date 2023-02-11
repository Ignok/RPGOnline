using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using RPGOnline.API.Helpers;
using RPGOnline.Application.DTOs.Requests.Forum;
using RPGOnline.Application.Interfaces;
using System.Security.Claims;

namespace RPGOnline.API.Controllers
{
    [Authorize]
    public class PostsController : CommonController
    {

        private readonly IPost _postService;
        private readonly BlobStorageConf _blobStorageConf;

        public PostsController(IPost postService, IOptions<BlobStorageConf> blobStorageConf)
        {
            _postService = postService;
            _blobStorageConf = blobStorageConf.Value;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetPosts([FromQuery] SearchPostRequest postRequest, CancellationToken cancellationToken)
        {
            try
            {
                var claimsIdentity = this.User.Identity as ClaimsIdentity;
                var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0";

                var result = await _postService.GetPosts(Int32.Parse(userId), postRequest, cancellationToken);



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

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPost(int id)
        {
            try
            {
                var claimsIdentity = this.User.Identity as ClaimsIdentity;
                var userId = claimsIdentity?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0";

                var result = await _postService.GetPostDetails(Int32.Parse(userId), id);

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

        [HttpDelete("delete/{postId}")]
        public async Task<IActionResult> DeletePost(int postId)
        {
            try
            {
                var claimsIdentity = this.User.Identity as ClaimsIdentity;
                var userId = claimsIdentity?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0";

                var result = await _postService.DeletePost(postId, Int32.Parse(userId), IsAdminRole());

                if (result == null)
                {
                    return BadRequest(result);
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("delete/comment/{commentId}")]
        public async Task<IActionResult> DeleteComment(int commentId)
        {
            try
            {
                var claimsIdentity = this.User.Identity as ClaimsIdentity;
                var userId = claimsIdentity?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0";

                var result = await _postService.DeleteComment(commentId, Int32.Parse(userId), IsAdminRole());

                if (result == null)
                {
                    return BadRequest(result);
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }



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

        [HttpGet("BlobToken/{uId}")]
        public async Task<IActionResult> GetBlobToken(int uId)
        {
            try
            {
                if (!IsSameId(uId))
                {
                    return BadRequest("Access denied - bad ID");
                }

                return Ok(_blobStorageConf);
            }
            catch(Exception ex)
            {
                return BadRequest(ex);
            }
        }

        private bool IsSameId(int id)
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return userId.Equals(id.ToString());
        }

        private bool IsAdminRole()
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            var userRole = claimsIdentity?.FindFirst(ClaimTypes.Role)?.Value ?? "a";

            return userRole.Equals("admin");
        }
    }
}
