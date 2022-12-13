using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RPGOnline.Application.DTOs.Requests;
using RPGOnline.Application.DTOs.Requests.Character;
using RPGOnline.Application.DTOs.Requests.Mail;
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

        [HttpGet("{uId}")]
        public async Task<IActionResult> GetRaces(int uId, [FromQuery] GetRaceRequest getRaceRequest)
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
    }
}
