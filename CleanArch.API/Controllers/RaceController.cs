using Microsoft.AspNetCore.Mvc;
using RPGOnline.Application.DTOs.Requests.Asset;
using RPGOnline.Application.DTOs.Requests.Asset.Race;
using RPGOnline.Application.Interfaces;

namespace RPGOnline.API.Controllers
{
    public class RaceController : CommonController
    {
        private readonly IRace _raceService;
        public RaceController(IRace raceService)
        {
            _raceService = raceService;
        }



        [HttpGet]
        public async Task<IActionResult> GetRaces([FromQuery] SearchAssetRequest searchRaceRequest, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _raceService.GetRaces(searchRaceRequest, cancellationToken);

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
    }
}
