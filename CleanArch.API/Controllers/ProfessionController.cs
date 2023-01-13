using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RPGOnline.Application.DTOs.Requests.Asset;
using RPGOnline.Application.DTOs.Requests.Asset.Profession;
using RPGOnline.Application.Interfaces;

namespace RPGOnline.API.Controllers
{
    public class ProfessionController : CommonController
    {
        private readonly IProfession _professionService;
        public ProfessionController(IProfession professionService)
        {
            _professionService = professionService;
        }

        [HttpGet]
        public async Task<IActionResult> GetProfessions([FromQuery] SearchAssetRequest searchProfessionRequest, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _professionService.GetProfessions(searchProfessionRequest, cancellationToken);

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
        public async Task<IActionResult> GetProfessionsForCharacter(int uId, [FromQuery] GetAssetForCharacterRequest getProfessionRequest)
        {
            try
            {
                var result = await _professionService.GetProfessionsForCharacter(uId, getProfessionRequest);

                if (result == null)
                {
                    return NotFound("No professions in database.");
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
        public async Task<IActionResult> PostProfession(PostProfessionRequest postProfessionRequest)
        {
            try
            {
                var result = await _professionService.PostProfession(postProfessionRequest);

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
