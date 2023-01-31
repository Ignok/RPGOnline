using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RPGOnline.Application.Interfaces;
using RPGOnline.Infrastructure.Services;

namespace RPGOnline.API.Controllers
{
    public class CharacterController : CommonController
    {
        private readonly ICharacter _characterService;

        public CharacterController(ICharacter characterService)
        {
            _characterService = characterService;
        }

        [AllowAnonymous]
        [HttpGet("character/motivation")]
        public async Task<IActionResult> GetMotivation()
        {
            try
            {
                var result = await _characterService.GetMotivation();

                if (result == null)
                {
                    return NotFound("No available motivations in database.");
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

        [AllowAnonymous]
        [HttpGet("character/characteristics")]
        public async Task<IActionResult> GetCharacteristics()
        {
            try
            {
                var result = await _characterService.GetCharacteristics();

                if (result == null)
                {
                    return NotFound("No available characteristics in database.");
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
