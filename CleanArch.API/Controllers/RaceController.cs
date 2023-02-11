using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RPGOnline.Application.DTOs.Requests.Asset;
using RPGOnline.Application.DTOs.Requests.Asset.Race;
using RPGOnline.Application.Interfaces;
using System.Security.Claims;

namespace RPGOnline.API.Controllers
{
    [Authorize]
    public class RaceController : CommonController
    {
        private readonly IRace _raceService;
        public RaceController(IRace raceService)
        {
            _raceService = raceService;
        }


        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetRaces([FromQuery] SearchAssetRequest searchRaceRequest, CancellationToken cancellationToken)
        {
            try
            {
                var claimsIdentity = this.User.Identity as ClaimsIdentity;
                var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0";

                var result = await _raceService.GetRaces(searchRaceRequest, Int32.Parse(userId), cancellationToken);

                return Ok(new
                {
                    result.Item1,
                    result.pageCount
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



        [HttpGet("character/{uId}")]
        public async Task<IActionResult> GetRacesForCharacter(int uId, [FromQuery] GetAssetForCharacterRequest getRaceRequest)
        {
            try
            {
                if (!IsSameId(uId))
                {
                    return BadRequest("Access denied - bad ID");
                }

                var result = await _raceService.GetRacesForCharacter(uId, getRaceRequest);

                if (result == null)
                {
                    return NotFound("No races in database.");
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


        [HttpPost]
        public async Task<IActionResult> PostRace(PostRaceRequest postRaceRequest)
        {
            try
            {
                if (!IsSameId(postRaceRequest.UId))
                {
                    return BadRequest("Access denied - bad ID");
                }

                var result = await _raceService.PostRace(postRaceRequest);

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

        private bool IsSameId(int id)
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return userId.Equals(id.ToString());
        }
    }
}
