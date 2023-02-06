using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using RPGOnline.Infrastructure.Models;
using System.Security.Claims;
using RPGOnline.Application.Interfaces;
using RPGOnline.Application.DTOs.Requests.Friendship;

namespace RPGOnline.API.Controllers
{
    [Authorize]
    public class FriendshipController : CommonController
    {
        private readonly RPGOnlineDbContext _dbContext;

        private readonly IFriendship _friendshipService;

        public FriendshipController(RPGOnlineDbContext dbContext, IFriendship friendshipService)
        {
            _friendshipService = friendshipService;
            _dbContext = dbContext;
        }


        // GET: api/{id}/Friends/{targetUId}
        [HttpGet("{uId}/{targetUId}")]
        public async Task<IActionResult> GetFriendship(int uId, int targetUId)
        {
            try
            {
                if (!IsSameId(uId))
                {
                    return BadRequest("Access denied - bad ID");
                }
                else if (!UserExists(targetUId))
                {
                    return NotFound("No such user in database");
                }
                else
                {
                    var result = await _friendshipService.GetFriendship(uId, targetUId);
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


        // GET: api/{id}/Friends
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserFriends(int id)
        {
            try
            {
                if (!UserExists(id))
                {
                    return NotFound("No such user in database");
                }
                else
                {
                    var claimsIdentity = this.User.Identity as ClaimsIdentity;
                    var userId = claimsIdentity?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                    if(userId == null)
                    {
                        return BadRequest("Cannot read user's ID");
                    }

                    var result = await _friendshipService.GetUserFriends(Int32.Parse(userId), id);
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

        [HttpPost("Manage")]
        public async Task<IActionResult> ManageFriendship(FriendshipRequest friendshipRequest)
        {
            try
            {
                if (!IsSameId(friendshipRequest.UId))
                {
                    return BadRequest("Access denied - bad ID");
                }

                if (friendshipRequest == null)
                {
                    throw new ArgumentNullException(nameof(friendshipRequest));
                }
                else if (!UserExists(friendshipRequest.TargetUId))
                {
                    return NotFound("No such user in database");
                }
                else
                {
                    var result = await _friendshipService.ManageFriendship(friendshipRequest);
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

        [HttpPost("Rate")]
        public async Task<IActionResult> ManageRating(FriendshipRatingRequest friendshipRatingRequest)
        {
            try
            {
                if (!IsSameId(friendshipRatingRequest.UId))
                {
                    return BadRequest("Access denied - bad ID");
                }

                if (friendshipRatingRequest == null)
                {
                    throw new ArgumentNullException(nameof(friendshipRatingRequest));
                }
                else if (!UserExists(friendshipRatingRequest.TargetUId))
                {
                    return NotFound("No such user in database");
                }
                else
                {
                    var result = await _friendshipService.ManageRating(friendshipRatingRequest);
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
            var userId = claimsIdentity?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null) return false;

            return userId.Equals(id.ToString());
        }
    }
}
