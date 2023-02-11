using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RPGOnline.Application.DTOs.Requests;
using RPGOnline.Application.DTOs.Requests.User;
using RPGOnline.Application.Interfaces;
using RPGOnline.Infrastructure.Models;

namespace RPGOnline.API.Controllers
{
    [Authorize]
    public class UsersController : CommonController
    {
        private readonly RPGOnlineDbContext _dbContext;

        private readonly IUser _userService;

        public UsersController(RPGOnlineDbContext dbContext, IUser userService)
        {
            _userService = userService;
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers([FromQuery] SearchUserRequest userRequest, CancellationToken cancellationToken)
        {
            try
            {
                var claimsIdentity = this.User.Identity as ClaimsIdentity;
                var userId = claimsIdentity?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0";

                var result = await _userService.GetUsers(userRequest, Int32.Parse(userId), cancellationToken);

                if (result == null)
                {
                    return NotFound("No users in database.");
                }
                else
                {
                    return Ok(result);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAboutMe(int id)
        {
            try
            {
                var claimsIdentity = this.User.Identity as ClaimsIdentity;
                var userId = claimsIdentity?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0";

                if(userId == "0")
                {
                    return BadRequest("Access denied - bad ID");
                }

                var result = await _userService.GetAboutMe(Int32.Parse(userId), id);
                if (result==null)
                {
                    return NotFound("No such user in database");
                }
                else
                {
                    return Ok(result);
                }
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPut("{id}/Details")]
        public async Task<IActionResult> PutUser(int id, UserRequest userRequest)
        {
            try
            {

                if (!IsSameId(id))
                {
                    return BadRequest("Access denied - bad ID");
                }


                var result = await _userService.PutUser(id, userRequest);
                return Ok(result);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound("No such user in database");
                }
                else
                {
                    return NoContent();
                }
            }
        }

        [HttpPut("{id}/Avatar")]
        public async Task<IActionResult> PutAvatar(int id, AvatarRequest avatarRequest)
        {
            try
            {
                if (!IsSameId(id))
                {
                    return BadRequest("Access denied - bad ID");
                }

                var result = await _userService.PutAvatar(id, avatarRequest);
                return Ok(result);
            }
            catch (DbUpdateConcurrencyException)
            {
                return NoContent();
            }
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                if (!IsSameId(id))
                {
                    return BadRequest("Access denied - bad ID");
                }

                var user = await _dbContext.Users.FindAsync(id);
                if (user == null)
                {
                    return NotFound("No such user in database");
                }
                else
                {
                    _dbContext.Users.Remove(user);
                    await _dbContext.SaveChangesAsync();

                    return NoContent();
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }



        [HttpPost("{uId}/Assets/{assetId}")]
        public async Task<IActionResult> PostSaveAsset(int uId, int assetId)
        {
            try
            {
                if (!IsSameId(uId))
                {
                    return NotFound("Access denied - bad ID");
                }
                else
                {
                    var result = await _userService.PostSaveAsset(uId, assetId);
                    if (result == null)
                    {
                        return NoContent();
                    }
                    return Ok(result);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpDelete("{uId}/Assets/{assetId}")]
        public async Task<IActionResult> DeleteSaveAsset(int uId, int assetId)
        {
            try
            {
                if (!IsSameId(uId))
                {
                    return NotFound("Access denied - bad ID");
                }
                else
                {
                    var result = await _userService.DeleteSaveAsset(uId, assetId);
                    if (result == null)
                    {
                        return NoContent();
                    }
                    return Ok(result);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("{uId}/Posts/{postId}")]
        public async Task<IActionResult> PostSavePost(int uId, int postId)
        {
            try
            {
                if (!IsSameId(uId))
                {
                    return NotFound("Access denied - bad ID");
                }
                else
                {
                    var result = await _userService.PostSavePost(uId, postId);
                    if (result == null)
                    {
                        return NoContent();
                    }
                    return Ok(result);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpDelete("{uId}/Posts/{postId}")]
        public async Task<IActionResult> DeleteSavePost(int uId, int postId)
        {
            try
            {
                if (!IsSameId(uId))
                {
                    return NotFound("Access denied - bad ID");
                }
                else
                {
                    var result = await _userService.DeleteSavePost(uId, postId);
                    if (result == null)
                    {
                        return NoContent();
                    }
                    return Ok(result);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        private bool UserExists(int id)
        {
            return _dbContext.Users.Any(e => e.UId == id);
        }

        private bool IsSameId(int id)
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return userId.Equals(id.ToString());
        }

    }
}
